using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Account.GetCurrentUser;
using UtilityPaymentJournal.Features.Account.SignIn;
using UtilityPaymentJournal.Features.Users.Create;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class AccountApiControllerTests : BaseIntegrationTest
    {
        public AccountApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при передаче верных учетных данных метод возвращает статус 200 OK 
        /// и объект ответа со статусом Success. Пользователь создается через эндпоинт Admin API.
        /// </summary>
        [Fact]
        public async Task SignIn_Should_ReturnOk_When_CredentialsAreValid()
        {
            // ==========================================
            // ARRANGE (Подготовка данных через API)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            string testPassword = "TestPassword123!";

            // 1. Создаем пользователя через Admin API. 
            // Запрос идет под правами фейкового админа из TestAuthHandler, поэтому создание разрешено.
            // Хэндлер сам создаст и привяжет роль, если её ещё нет в БД, а также выставит EmailConfirmed = true.
            var createUserRequest = new CreateUserRequest(
                testUserName,
                "testFirstName",
                "testLastName",
                testPassword,
                UserRole.User
            );

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            createResponse.IsSuccessStatusCode.Should().BeTrue();

            // Извлекаем созданного пользователя из ответа, чтобы получить его сгенерированный ID
            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // КРИТИЧЕСКИ ВАЖНО: Сбрасываем ChangeTracker и принудительно вычитываем пользователя из PostgreSQL.
            // Это гарантирует, что EF Core в тестовом процессе зафиксирует состояние флага EmailConfirmed = true
            // и передаст актуальные данные в параллельный HTTP-контекст для SignInManager.
            DbContext.ChangeTracker.Clear();
            var userInDb = await DbContext.Users.FindAsync(createdUser!.Id);
            userInDb.Should().NotBeNull();
            userInDb!.EmailConfirmed = true; // Подстраховка на случай, если в хэндлере флаг не успел примениться
            await DbContext.SaveChangesAsync();

            DbContext.ChangeTracker.Clear();

            // 2. Готовим DTO запроса на аутентификацию
            SignInRequest signInRequest = new SignInRequest(testUserName, testPassword, IsPersistent: false);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            // Запрос отправляется напрямую. StubAntiforgery отключает CSRF-проверку, 
            // а логика входа проверит именно те данные, что лежат в signInRequest.
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/account/sign-in", signInRequest);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка при входе: {errorText}");
            }

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ и сверяем бизнес-статусы вашей DTO структуры SignInResponse
            SignInResponse? responseContent = await response.Content.ReadFromJsonAsync<SignInResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.IsSuccess.Should().BeTrue();
            responseContent.Status.Should().Be(SignInResultStatus.Success);
            responseContent.ErrorMessage.Should().BeNull();
        }

        /// <summary>
        /// Проверяет, что при передаче неверного пароля контроллер возвращает 
        /// статус 401 Unauthorized и бизнес-статус InvalidCredentials с сообщением об ошибке.
        /// </summary>
        [Fact]
        public async Task SignIn_Should_ReturnUnauthorized_When_PasswordIsIncorrect()
        {
            // ==========================================
            // ARRANGE (Подготовка данных через API)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            string testPassword = "CorrectPassword123!";

            // 1. Создаем пользователя через Admin API
            CreateUserRequest createUserRequest = new CreateUserRequest(
                testUserName,
                "testName",
                "testLastName",
                testPassword,
                UserRole.User
            );

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            if (!createResponse.IsSuccessStatusCode)
            {
                string createError = await createResponse.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[Arrange Failed] Не удалось создать пользователя через Admin API: {createError}");
            }

            // Извлекаем созданного пользователя, чтобы узнать его сгенерированный ID
            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // КРИТИЧЕСКИ ВАЖНО: Сбрасываем трекер и явно подтверждаем Email в базе данных.
            // Это гарантирует, что SignInManager пропустит шаг RequireConfirmedEmail и перейдет к сверке паролей.
            DbContext.ChangeTracker.Clear();
            var userInDb = await DbContext.Users.FindAsync(createdUser!.Id);
            userInDb.Should().NotBeNull();
            userInDb!.EmailConfirmed = true;
            await DbContext.SaveChangesAsync();

            DbContext.ChangeTracker.Clear();

            // 2. Формируем запрос на вход, передавая ЗАВЕДОМО НЕВЕРНЫЙ абстрактный пароль
            SignInRequest signInRequest = new SignInRequest(testUserName, "wrong_password_999", IsPersistent: false);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/account/sign-in", signInRequest);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный HTTP-статус 401 Unauthorized для неверных учетных данных
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Десериализуем ответ для проверки структуры SignInResponse
            SignInResponse? responseContent = await response.Content.ReadFromJsonAsync<SignInResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.IsSuccess.Should().BeFalse();
            responseContent.Status.Should().Be(SignInResultStatus.InvalidCredentials);
            responseContent.ErrorMessage.Should().Be("Неверный логин или пароль.");
        }

        /// <summary>
        /// Проверяет, что если аккаунт пользователя заблокирован из-за превышения попыток входа,
        /// контроллер возвращает статус 400 BadRequest и бизнес-статус LockedOut с соответствующей ошибкой.
        /// </summary>
        [Fact]
        public async Task SignIn_Should_ReturnBadRequest_When_AccountIsLockedOut()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            string testPassword = "CorrectPassword123!";

            // 1. Создаем пользователя через Admin API
            var createUserRequest = new CreateUserRequest(
                testUserName,
                "testName",
                "testLastName",
                testPassword,
                UserRole.User
            );

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            if (!createResponse.IsSuccessStatusCode)
            {
                string createError = await createResponse.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[Arrange Failed] Не удалось создать пользователя через Admin API: {createError}");
            }

            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // 2. Находим созданного пользователя в БД через DbContext и имитируем его блокировку
            DbContext.ChangeTracker.Clear();
            var userInDb = await DbContext.Users.FindAsync(createdUser!.Id);
            userInDb.Should().NotBeNull();

            // ВАЖНО: Принудительно подтверждаем Email, чтобы Identity не остановил пайплайн входа на ошибке NotAllowed,
            // а честно дошел до проверки временного бана в поле LockoutEnd.
            userInDb!.EmailConfirmed = true;

            // Выставляем окончание блокировки на 15 минут вперед от текущего времени
            userInDb.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(15);

            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 3. Формируем запрос с верными данными, но для заблокированного аккаунта
            SignInRequest request = new SignInRequest(testUserName, testPassword, IsPersistent: false);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/account/sign-in", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 400 BadRequest согласно вашей логике контроллера
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Десериализуем ответ для проверки структуры SignInResponse
            SignInResponse? responseContent = await response.Content.ReadFromJsonAsync<SignInResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.IsSuccess.Should().BeFalse();
            responseContent.Status.Should().Be(SignInResultStatus.LockedOut);
            responseContent.ErrorMessage.Should().Be("Аккаунт временно заблокирован из-за множества неудачных попыток входа.");
        }

        /// <summary>
        /// Проверяет, что если доступ к системе ограничен бизнес-логикой (например, неподтвержденный Email),
        /// контроллер возвращает статус 400 BadRequest и бизнес-статус NotAllowed с сообщением об ограничении.
        /// </summary>
        [Fact]
        public async Task SignIn_Should_ReturnBadRequest_When_AccessIsNotAllowed()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            string testPassword = "CorrectPassword123!";

            // 1. Создаем пользователя через Admin API (с автоматической привязкой роли)
            var createUserRequest = new CreateUserRequest(
                testUserName,
                "testName",
                "testLastName",
                testPassword,
                UserRole.User
            );

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            createResponse.IsSuccessStatusCode.Should().BeTrue();

            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // 2. Напрямую сбрасываем подтверждение почты в БД для честной симуляции статуса NotAllowed
            DbContext.ChangeTracker.Clear();
            var userInDb = await DbContext.Users.FindAsync(createdUser!.Id);
            userInDb.Should().NotBeNull();
            userInDb!.EmailConfirmed = false;
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 3. Формируем стандартный DTO запроса на вход
            SignInRequest request = new SignInRequest(testUserName, testPassword, IsPersistent: false);

            // ==========================================
            // ACT (Выполнение запроса через HttpClient)
            // ==========================================
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/account/sign-in", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Статус должен быть честным 400 BadRequest
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            SignInResponse? responseContent = await response.Content.ReadFromJsonAsync<SignInResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.IsSuccess.Should().BeFalse();
            responseContent.Status.Should().Be(SignInResultStatus.NotAllowed);
            responseContent.ErrorMessage.Should().Be("Доступ к системе ограничен. Обратитесь к администратору.");
        }

        /// <summary>
        /// Проверяет, что метод выхода из системы успешно завершает сессию пользователя 
        /// и возвращает HTTP-статус 200 OK без тела ответа.
        /// </summary>
        [Fact]
        public async Task SignOut_Should_ClearSession_And_ReturnOkStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных под TestAuthHandler)
            // ==========================================
            const string testUserId = "test-admin-id-123";

            // Гарантируем наличие записи с ID из TestAuthHandler в базе PostgreSQL
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                User testUser = new User
                {
                    Id = testUserId,
                    UserName = "TestAdmin",
                    FirstName = "testFirstName",
                    LastName = "testLastName",
                    // Явно подтверждаем Email, чтобы глобальная подсистема безопасности Identity 
                    // пропустила наш запрос на выход
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                await DbContext.Users.AddAsync(testUser);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }
            else
            {
                // Подстраховка: если юзер уже был создан, принудительно активируем его
                DbContext.ChangeTracker.Clear();
                var existingUser = await DbContext.Users.FindAsync(testUserId);
                if (existingUser != null && !existingUser.EmailConfirmed)
                {
                    existingUser.EmailConfirmed = true;
                    await DbContext.SaveChangesAsync();
                }
                DbContext.ChangeTracker.Clear();
            }

            // Для POST-запроса SignOut передаем пустое тело
            HttpContent emptyContent = new StringContent(string.Empty);

            // ==========================================
            // ACT (Выполнение запроса на выход)
            // ==========================================
            // Запрос идет под фейковым админом из TestAuthHandler. 
            // Метод очистит сессию/вызовет команду и вернет статус.
            HttpResponseMessage response = await Client.PostAsync("api/account/sign-out", emptyContent);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK согласно логике контроллера
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Проверяет, что текущий аутентифицированный через TestAuthHandler пользователь 
        /// успешно получает информацию о своем профиле со статусом 200 OK.
        /// </summary>
        [Fact]
        public async Task GetCurrentUser_Should_ReturnProfileData_When_UserIsAuthenticated()
        {
            // ==========================================
            // ARRANGE (Подготовка данных в БД)
            // ==========================================

            // Берем ID, который жестко зашит в вашем TestAuthHandler
            const string testUserId = "test-admin-id-123";
            const string testUserName = "TestAdmin";
            const string testFirstName = "testFirstName";
            const string testLastName = "testLastName";

            // Гарантируем, что в СУБД физически существует запись, иначе маппер или логика
            // получения профиля вернет NotFound/ошибку, не найдя строку по ID из токена.
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                User testUser = new User
                {
                    Id = testUserId,
                    UserName = testUserName,
                    FirstName = testFirstName,
                    LastName = testLastName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                await DbContext.Users.AddAsync(testUser);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // ==========================================
            // ACT (Выполнение запроса — авторизация подставится автоматически через TestAuthHandler!)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/account/current");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetCurrentUserResponse? content = await response.Content.ReadFromJsonAsync<GetCurrentUserResponse>();
            content.Should().NotBeNull();

            // Сверяем данные DTO с тем, что зашито в TestAuthHandler и сохранено в базе
            content!.Id.Should().Be(testUserId);
            content.UserName.Should().Be(testUserName);
            content.FirstName.Should().Be(testFirstName);
            content.LastName.Should().Be(testLastName);
        }
    }
}
