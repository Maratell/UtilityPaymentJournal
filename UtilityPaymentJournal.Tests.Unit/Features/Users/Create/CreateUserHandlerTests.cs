using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Common.Exceptions;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Features.Users.Create;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Tests.Unit.Features.Users.Create
{
    public class CreateUserHandlerTests
    {
        #region ВНЕШНИЕ ЗАВИСИМОСТИ (MOCKS / ЗАГЛУШКИ)

        // Каноничный мокинг интерфейса одной строчкой. Так как IApplicationDbContext, ILogger, IDbContextTransaction — это интерфейсы, 
        // NSubstitute создаёт их прокси-версию в памяти без каких-либо предупреждений анализатора.
        private readonly IApplicationDbContext _context = Substitute.For<IApplicationDbContext>();
        private readonly ILogger<CreateUserHandler> _logger = Substitute.For<ILogger<CreateUserHandler>>();
        // Мок виртуальной транзакции. Мы будем проверять поведение этого объекта в секции Assert: 
        // вызвался ли '.CommitAsync()' при успехе или '.RollbackAsync()' при возникновении ошибок.
        private readonly IDbContextTransaction _transaction = Substitute.For<IDbContextTransaction>();

        // Вынужденное исключение из правил: 'UserManager' и 'RoleManager' — это тяжелые конкретные классы 
        // фреймворка ASP.NET Core Identity. Их конструкторы требуют кучу параметров (хранилища, хэшеры, валидаторы).
        // Чтобы NSubstitute не упал при инициализации, мы создаем их через кастомные фабричные методы.
        private readonly UserManager<User> _userManager = CreateUserManagerMock();
        private readonly RoleManager<Role> _roleManager = CreateRoleManagerMock();

        #endregion

        // Специальный фасад Entity Framework для управления низкоуровневыми операциями базы данных.
        // Нужен для перехвата вызовов транзакций, чтобы код хэндлера в блоках 'using' и 'catch' работал без сбоев.
        private readonly DatabaseFacade _databaseFacade;
        // Тестируемый объект (System Under Test — SUT). Реальный экземпляр хэндлера, 
        // в который мы принудительно внедряем все наши изолированные заглушки через конструктор.
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _databaseFacade = Substitute.For<DatabaseFacade>((Microsoft.EntityFrameworkCore.DbContext)null!);

            _databaseFacade.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(_transaction);
            _context.Database.Returns(_databaseFacade);

            _handler = new CreateUserHandler(_context, _userManager, _roleManager, _logger);
        }

        /// <summary>
        /// Проверяет, что при успешном выполнении всех шагов (создание пользователя, проверка роли 
        /// и привязка роли) хэндлер возвращает заполненный объект ответа, а транзакция базы данных 
        /// успешно фиксируется (вызывается Commit) без аварийного отката (Rollback).
        /// </summary>
        [Fact]
        public async Task Handle_Should_CreateUserAndCommitTransaction_When_AllStepsSucceed()
        {
            // =========================================================================================================
            // ARRANGE (ПОДГОТОВКА: Формируем входные данные и жестко настраиваем поведение наших заглушек-моков)
            // =========================================================================================================

            // Формируем команду для обработчика (хэндлера)
            CreateUserCommand command = new CreateUserCommand(
                UserName: "TestUserName",
                FirstName: "TestFirstName",
                LastName: "TestLastName", 
                Password: "TestPassword123!", 
                Role: UserRole.User);

            string expectedRoleName = command.Role.GetDisplayName();

            // Шаг 1 в хэндлере: Заставляем UserManager поверить, что создание пользователя в таблице AspNetUsers прошло успешно
            // 'Arg.Any<User>()' означает: "Нам не важно, какой именно объект User создался внутри хэндлера, просто верни Success"
            _userManager.CreateAsync(Arg.Any<User>(), command.Password).Returns(IdentityResult.Success);

            // Шаг 2 в хэндлере: Имитируем, что нужная роль уже физически существует в таблице AspNetRoles (возвращаем true)
            _roleManager.RoleExistsAsync(expectedRoleName).Returns(true);

            // Шаг 3 в хэндлере: Говорим UserManager, что привязка созданного пользователя к этой роли завершилась успехом
            _userManager.AddToRoleAsync(Arg.Any<User>(), expectedRoleName).Returns(IdentityResult.Success);

            // =========================================================================================================
            // ACT (ДЕЙСТВИЕ: Запускаем выполнение тестируемого метода хэндлера в изолированной среде)
            // =========================================================================================================

            // Передаем команду напрямую в метод Handle нашего хэндлера и ждем возвращаемый DTO ответ
            CreateUserResponse result = await _handler.Handle(command, CancellationToken.None);

            // =========================================================================================================
            // ASSERT (ПРОВЕРКА: Сверяем полученный результат и контролируем внутреннее поведение транзакций базы данных)
            // =========================================================================================================

            // Проверяем, что хэндлер успешно отработал до конца и вернул наполненный объект ответа (не null)
            result.Should().NotBeNull();

            // КРИТИЧЕСКИ ВАЖНО ДЛЯ ТРАНЗАКЦИЙ: С помощью NSubstitute проверяем, что метод 'CommitAsync' у нашей транзакции 
            // был вызван ровно 1 раз. Это доказывает, что все шаги выполнились без сбоев и данные физически сохранены в БД.
            await _transaction.Received(1).CommitAsync(Arg.Any<CancellationToken>());

            // ПОДСТРАХОВКА: Проверяем поведение отката. Метод 'RollbackAsync' ни разу (DidNotReceive) не должен быть вызван, 
            // так как в успешном сценарии база данных не должна стирать или откатывать внесенные изменения.
            await _transaction.DidNotReceive().RollbackAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Проверяет, что если на первом шаге (создание пользователя в Identity) произошел сбой,
        /// метод Handle прерывает работу, выбрасывает кастомное исключение с ошибками валидации,
        /// а транзакция базы данных принудительно откатывается (вызывается Rollback).
        /// </summary>
        [Fact]
        public async Task Handle_Should_RollbackTransaction_And_ThrowException_When_UserCreationFails()
        {
            // =========================================================================================================
            // ARRANGE (ПОДГОТОВКА: Моделируем аварийную ситуацию на самом первом шаге работы хэндлера)
            // =========================================================================================================

            // Формируем команду для обработчика (хэндлера)
            CreateUserCommand command = new CreateUserCommand(
                UserName: "TestUserName",
                FirstName: "TestFirstName",
                LastName: "TestLastName",
                Password: "TestPassword123!",
                Role: UserRole.User);

            // Формируем фейковую ошибку Identity (например, пароль слишком короткий или логин занят)
            IdentityError identityError = new IdentityError { Description = "Пароль слишком короткий." };

            // Имитируем сбой UserManager на Шаге 1: возвращаем результат со статусом Failed
            _userManager.CreateAsync(Arg.Any<User>(), command.Password)
                .Returns(Task.FromResult(IdentityResult.Failed(identityError)));

            // Настраиваем фасад базы данных, чтобы свойство CurrentTransaction возвращало наш мок транзакции для блока catch
            _databaseFacade.CurrentTransaction.Returns(_transaction);

            // =========================================================================================================
            // ACT (ДЕЙСТВИЕ: Упаковываем вызов метода хэндлера в отложенное действие-делегат)
            // =========================================================================================================

            // Используем лямбда-выражение, так как мы ожидаем падение метода и хотим перехватить исключение в блоке Assert
            // Это отложенное действие не позволяет методу упасть с ошибкой здесь
            Func<Task<CreateUserResponse>> act = async () => await _handler.Handle(command, CancellationToken.None);

            // =========================================================================================================
            // ASSERT (ПРОВЕРКА: Контролируем тип ошибки и обязательный аварийный откат изменений из БД)
            // =========================================================================================================

            // Проверяем, что хэндлер выбросил именно ваше кастомное исключение валидации Identity
            await act.Should().ThrowAsync<IdentityValidationException>();

            // КРИТИЧЕСКИ ВАЖНО: Проверяем, что из-за ошибки транзакция вызвала RollbackAsync ровно 1 раз
            await _transaction.Received(1).RollbackAsync(Arg.Any<CancellationToken>());

            // ПОДСТРАХОВКА: Убеждаемся, что метод фиксации данных CommitAsync ни разу не вызывался
            await _transaction.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Проверяет, что если запрашиваемой роли нет в системе и попытка её автоматического 
        /// создания завершилась ошибкой, хэндлер прерывает операцию, выбрасывает исключение валидации,
        /// а уже созданный на первом шаге пользователь успешно стирается из базы через откат транзакции.
        /// </summary>
        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_RoleCreationFails()
        {
            // =========================================================================================================
            // ARRANGE (ПОДГОТОВКА: Симулируем успешный первый шаг, но ломаем логику создания роли на втором)
            // =========================================================================================================

            var command = new CreateUserCommand(
                UserName: "TestUserName",
                FirstName: "TestFirstName",
                LastName: "TestLastName",
                Password: "TestPassword123!",
                Role: UserRole.User);

            string expectedRoleName = command.Role.GetDisplayName();

            // Шаг 1: Пользователь успешно создается фреймворком Identity
            _userManager.CreateAsync(Arg.Any<User>(), command.Password).Returns(IdentityResult.Success);

            // Шаг 2 (Часть 1): Имитируем, что запрашиваемой роли еще нет в таблице AspNetRoles (возвращаем false)
            _roleManager.RoleExistsAsync(expectedRoleName).Returns(false);

            // Шаг 2 (Часть 2): Настраиваем сбой RoleManager при попытке добавить новую роль в базу данных
            IdentityError roleError = new IdentityError { Description = "Ошибка создания роли." };
            _roleManager.CreateAsync(Arg.Any<Role>()).Returns(IdentityResult.Failed(roleError));

            // Предоставляем активную транзакцию для безопасной отработки блока catch внутри хэндлера
            _databaseFacade.CurrentTransaction.Returns(_transaction);

            // =========================================================================================================
            // ACT (ДЕЙСТВИЕ: Готовим делегат для перехвата исключения)
            // =========================================================================================================

            // Используем лямбда-выражение, так как мы ожидаем падение метода и хотим перехватить исключение в блоке Assert
            // Это отложенное действие не позволяет методу упасть с ошибкой здесь
            Func<Task<CreateUserResponse>> act = async () => await _handler.Handle(command, CancellationToken.None);

            // =========================================================================================================
            // ASSERT (ПРОВЕРКА: Проверяем изоляцию данных и отмену операции)
            // =========================================================================================================

            // Убеждаемся, что выполнение прервалось правильным типом кастомного исключения
            await act.Should().ThrowAsync<IdentityValidationException>();

            // Проверяем, что транзакция была аварийно откатана, аннулируя создание пользователя на первом шаге
            await _transaction.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Проверяет, что если пользователь создан и роль существует, но финальный шаг привязки 
        /// пользователя к этой роли упал, хэндлер выбрасывает исключение, а транзакция полностью 
        /// откатывает все изменения, предотвращая появление в системе "битого" аккаунта без роли.
        /// </summary>
        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_RoleAssignmentFails()
        {
            // =========================================================================================================
            // ARRANGE (ПОДГОТОВКА: Успешно проходим первые два шага, но ломаем финальное связывание сущностей)
            // =========================================================================================================

            var command = new CreateUserCommand(
                UserName: "TestUserName",
                FirstName: "TestFirstName",
                LastName: "TestLastName",
                Password: "TestPassword123!",
                Role: UserRole.User);

            string expectedRoleName = command.Role.GetDisplayName();

            // Шаг 1: Имитируем успешное создание записи пользователя
            _userManager.CreateAsync(Arg.Any<User>(), command.Password).Returns(IdentityResult.Success);

            // Шаг 2: Имитируем, что роль найдена и готова к привязке
            _roleManager.RoleExistsAsync(expectedRoleName).Returns(true);

            // Шаг 3: Имитируем критический сбой во внутренностях Identity при попытке записать связь в AspNetUserRoles
            var assignmentError = new IdentityError { Description = "Ошибка привязки роли." };
            _userManager.AddToRoleAsync(Arg.Any<User>(), expectedRoleName).Returns(IdentityResult.Failed(assignmentError));

            // Передаем транзакцию фасаду для блока catch
            _databaseFacade.CurrentTransaction.Returns(_transaction);

            // =========================================================================================================
            // ACT (ДЕЙСТВИЕ)
            // =========================================================================================================

            // Используем лямбда-выражение, так как мы ожидаем падение метода и хотим перехватить исключение в блоке Assert
            // Это отложенное действие не позволяет методу упасть с ошибкой здесь
            Func<Task<CreateUserResponse>> act = async () => await _handler.Handle(command, CancellationToken.None);

            // =========================================================================================================
            // ASSERT (ПРОВЕРКА)
            // =========================================================================================================

            // Метод обязан выбросить кастомную ошибку валидации
            await act.Should().ThrowAsync<IdentityValidationException>();

            // Проверяем, что даже на самом последнем шаге сбой гарантированно стер все временные изменения из БД
            await _transaction.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        }


        #region Вспомогательные элементы
        private static UserManager<User> CreateUserManagerMock()
        {
            var store = Substitute.For<IUserStore<User>>();
            return Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        }

        private static RoleManager<Role> CreateRoleManagerMock()
        {
            var store = Substitute.For<IRoleStore<Role>>();
            return Substitute.For<RoleManager<Role>>(store, null, null, null, null);
        }
        #endregion
    }
}
