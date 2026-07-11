using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.DTOs.Admin;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Exceptions;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserMapper _userMapper;
        private readonly ApplicationDbContext _dbContext;

        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUserMapper userMapper,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Менеджеры Identity сразу сохраняют каждое действие в БД через SaveChanges.
            // Чтобы при ошибке привязки роли в базе не оставался "битый" пользователь без роли,
            // мы объединяем все три шага в одну неделимую транзакцию: или запишется всё, или ничего.
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                // try нужен для того, чтобы перехватить любой сбой прямо в процессе выполнения шагов.
                // Без этого блока мы не смогли бы вызвать асинхронный откат (RollbackAsync), и серверу 
                // пришлось бы блокировать свои потоки, выполняя отмену синхронно внутри механизма using.
                try
                {
                    // Шаг 1: Создаем пользователя в таблице AspNetUsers
                    User user = new User
                    {
                        UserName = createDto.UserName,
                        FirstName = createDto.FirstName,
                        LastName = createDto.LastName
                    };

                    IdentityResult createdUser = await _userManager.CreateAsync(user, createDto.Password);
                    if (!createdUser.Succeeded)
                    {
                        // Вместо InvalidOperationException и склейки строк выбрасываем кастомное исключение валидации.
                        // IdentityValidationExceptionHandler перехватит его и вернет красивый 400 BadRequest.
                        throw new IdentityValidationException(createdUser.Errors.Select(e => e.Description));
                    }

                    // Шаг 2: Гарантируем наличие роли в таблице AspNetRoles
                    string roleName = createDto.Role.GetDisplayName();
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var roleResult = await _roleManager.CreateAsync(new Role(roleName));
                        if (!roleResult.Succeeded)
                            throw new IdentityValidationException(roleResult.Errors.Select(e => e.Description));
                    }

                    // Шаг 3: Привязываем пользователя к роли в связующей таблице AspNetUserRoles
                    var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!addRoleResult.Succeeded)
                        throw new IdentityValidationException(addRoleResult.Errors.Select(e => e.Description));

                    cancellationToken.ThrowIfCancellationRequested();

                    // Если все шаги успешны — подтверждаем изменения и сохраняем их на диск
                    await transaction.CommitAsync(cancellationToken);

                    return _userMapper.ToDto(user, roleName);
                }
                catch
                {
                    // При любой ошибке (включая наше исключение валидации) полностью стираем временные изменения из БД
                    if (_dbContext.Database.CurrentTransaction != null)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                    }
                    throw; // Передаем исключение дальше. Исключение валидации уйдет в свой Handler, остальные — в системные
                }
            }
        }

        public async Task<UserDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();

            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            cancellationToken.ThrowIfCancellationRequested();

            IList<string> roles = await _userManager.GetRolesAsync(user);
            string roleName = roles.FirstOrDefault() ?? "Нет роли";

            return _userMapper.ToDto(user, roleName);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // 1. Проблема N+1: Мы умышленно не используем цикл foreach с вызовом _userManager.GetRolesAsync(user) внутри,
            //    так как это привело бы к отправке N изолированных запросов к БД (N пользователей = N+1 запрос).
            // 2. Невозможность Include: Стандартный UserManager.Users не имеет навигационных свойств для ролей из коробки,
            //    поэтому мы не можем написать лаконичный жадный запрос вида .Include(u => u.UserRoles).
            // 3. Проблема Декартова произведения: Обычный LEFT JOIN таблиц пользователей и ролей дублирует строки главной 
            //    сущности, если у пользователя привязано несколько ролей (один человек размножится на 3 строки в таблице).
            // РЕШЕНИЕ: Мы пишем один эффективный SQL-запрос с явными LEFT JOIN и обязательной группировкой .GroupBy() 
            // на стороне СУБД. Это позволяет за один шаг выгрузить данные без N+1 и декартова размножения строк.
            var usersQuery = await (
                // 1. Указываем базовую таблицу пользователей (AspNetUsers) в качестве источника
                from user in _dbContext.Users

                // 2. Делаем LEFT JOIN со связующей таблицей AspNetUserRoles по ID пользователя
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если у него нет роли

                // 3. Делаем второй LEFT JOIN с таблицей ролей AspNetRoles по ID роли из связующей таблицы
                join role in _dbContext.Roles on ur.RoleId equals role.Id into rGroup
                from r in rGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если назначенная роль не найдена

                // 4. Проектируем промежуточный плоский результат, где строки могут дублироваться (Декартово произведение)
                select new { user, r.Name }
            )
                // 5. РЕШЕНИЕ ПРОБЛЕМЫ ДЕКАРТОВА ПРОИЗВЕДЕНИЯ: Группируем результат на стороне БД по уникальному объекту пользователя.
                // Это гарантирует, что база данных вернет строго ОДНУ строку на каждого человека, сколько бы ролей у него ни было.
                .GroupBy(x => x.user)

                // 6. Формируем структуру ответа из сгруппированных данных на сервере БД
                .Select(g => new
                {
                    User = g.Key, // Извлекаем самого пользователя (он был ключом группировки)

                    // Собираем все его роли в список, отсекая возможные null-значения
                    RoleNames = g.Select(x => x.Name).Where(name => name != null).ToList()
                })

                // 7. ОТКЛЮЧЕНИЕ ОТСЛЕЖИВАНИЯ: Говорим EF Core не тратить ресурсы на кэширование этих сущностей, 
                // так как мы их только читаем и не собираемся изменять в БД (ускоряет работу памяти)
                .AsNoTracking()

                // 8. РЕШЕНИЕ ПРОБЛЕМЫ N+1: Выполняем весь собранный выше SQL-запрос за ОДИН асинхронный шаг.
                // Передаем токен отмены напрямую в драйвер базы данных, чтобы прервать операцию при отмене со стороны UI.
                .ToListAsync(cancellationToken);

            // 9. МАППИНГ: Перебираем полученный чистый список в оперативной памяти и превращаем его в конечные DTO.
            // Если у пользователя было несколько ролей, для RoleName берем первую (или "Нет роли"), избегая дублирования строк.
            var userDtos = usersQuery.Select(x => _userMapper.ToDto(
                x.User,
                x.RoleNames.FirstOrDefault() ?? "Нет роли"
            )).ToList();

            // Возвращаем полностью готовый, оптимизированный список DTO
            return userDtos;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();

            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false; // Контроллер превратит это в 404 NotFound
            }

            cancellationToken.ThrowIfCancellationRequested();

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new IdentityValidationException(result.Errors.Select(e => e.Description));
            }

            return true;
        }
    }
}
