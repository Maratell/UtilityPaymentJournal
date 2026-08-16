using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Users.Queries
{
    public partial class UserQueryService(
        ApplicationDbContext dbContext,
        IUserMapper userMapper,
        ILogger<UserQueryService> logger) : IUserQueryService
    {
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUserMapper _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        private readonly ILogger<UserQueryService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<UserQueryResultDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            LogFetchingUserById(_logger, id);
            // ОПТИМИЗАЦИЯ ЗАПРОСА (CQRS Чтение):
            // 1. Избегаем N+1 и лишних сетевых задержек: Стандартный UserManager потребовал бы два раздельных 
            //    запроса (FindByIdAsync + GetRolesAsync). Мы пишем один лаконичный LINQ-запрос с явными 
            //    LEFT JOIN, заставляя базу данных соединить таблицы AspNetUsers, AspNetUserRoles и AspNetRoles 
            //    на своей стороне и вернуть весь результат за ОДИН единственный сетевой round-trip.
            // 2. Отключаем кэш слежения (.AsNoTracking()): Поскольку это операция чтения (Query), нам не нужно 
            //    изменять состояние сущности. Мы говорим EF Core не тратить оперативную память и ресурсы CPU 
            //    на создание тяжелых трекеров изменений (Change Tracker), что существенно ускоряет выполнение.
            var userData = await (
                from user in _dbContext.Users.AsNoTracking()
                where user.Id == id

                // Делаем первый LEFT JOIN со связующей таблицей, чтобы не потерять пользователя, если у него нет роли
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty()

                // Делаем второй LEFT JOIN с таблицей ролей для получения текстового названия роли
                join role in _dbContext.Roles on ur.RoleId equals role.Id into rGroup
                from r in rGroup.DefaultIfEmpty()

                // Формируем  легкий анонимный объект, вытягивая сущность и вычисленное имя роли
                select new { user, RoleName = r != null ? r.Name : "Нет роли" }
            ).FirstOrDefaultAsync(cancellationToken);

            if (userData is null)
            {
                LogUserNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Учетная запись пользователя с идентификатором {id} не найдена в БД.");
            }

            LogUserSuccessfullyFetchedFromDb(_logger, id, userData.RoleName);
            return _userMapper.ToQueryResultDto(userData.user, userData.RoleName);
        }

        public async Task<IReadOnlyCollection<UserQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllUsers(_logger);
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
                // 2. Отключаем кэш слежения (.AsNoTracking()): Поскольку это операция чтения (Query), нам не нужно 
                //    изменять состояние сущности. Мы говорим EF Core не тратить оперативную память и ресурсы CPU 
                //    на создание тяжелых трекеров изменений (Change Tracker), что существенно ускоряет выполнение.
                from user in _dbContext.Users.AsNoTracking()

                // 3. Делаем LEFT JOIN со связующей таблицей AspNetUserRoles по ID пользователя
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если у него нет роли

                // 4. Делаем второй LEFT JOIN с таблицей ролей AspNetRoles по ID роли из связующей таблицы
                join role in _dbContext.Roles on ur.RoleId equals role.Id into rGroup
                from r in rGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если назначенная роль не найдена

                // 5. Проектируем промежуточный плоский результат, где строки могут дублироваться (Декартово произведение)
                select new { user, r.Name }
            )
                // 6. РЕШЕНИЕ ПРОБЛЕМЫ ДЕКАРТОВА ПРОИЗВЕДЕНИЯ: Группируем результат на стороне БД по уникальному объекту пользователя.
                // Это гарантирует, что база данных вернет строго ОДНУ строку на каждого человека, сколько бы ролей у него ни было.
                .GroupBy(x => x.user)

                // 7. Формируем структуру ответа из сгруппированных данных на сервере БД
                .Select(g => new
                {
                    User = g.Key, // Извлекаем самого пользователя (он был ключом группировки)

                    // Собираем все его роли в список, отсекая возможные null-значения
                    RoleNames = g.Select(x => x.Name).Where(name => name != null).ToList()
                })

                // 8. РЕШЕНИЕ ПРОБЛЕМЫ N+1: Выполняем весь собранный выше SQL-запрос за ОДИН асинхронный шаг.
                // Передаем токен отмены напрямую в драйвер базы данных, чтобы прервать операцию при отмене со стороны UI.
                .ToListAsync(cancellationToken);

            LogAllUsersSuccessfullyFetchedFromDb(_logger, usersQuery.Count);
            // 9. МАППИНГ: Перебираем полученный чистый список в оперативной памяти и превращаем его в конечные DTO.
            // Если у пользователя было несколько ролей, для RoleName берем первую (или "Нет роли"), избегая дублирования строк.
            return usersQuery.Select(x => _userMapper.ToQueryResultDto(
                x.User,
                x.RoleNames.FirstOrDefault() ?? "Нет роли"
            )).ToList().AsReadOnly();
        }
    }
}
