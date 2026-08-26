using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Users.GetList
{
    public partial class GetUsersListHandler(
            ApplicationDbContext context,
            ILogger<GetUsersListHandler> logger) : IRequestHandler<GetUsersListQuery, GetUsersListResponse>
    {
        public async Task<GetUsersListResponse> Handle(GetUsersListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllUsers(logger);
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
                from user in context.Users.AsNoTracking()

                // 3. Делаем LEFT JOIN со связующей таблицей AspNetUserRoles по ID пользователя
                join userRole in context.UserRoles on user.Id equals userRole.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если у него нет роли

                // 4. Делаем второй LEFT JOIN с таблицей ролей AspNetRoles по ID роли из связующей таблицы
                join role in context.Roles on ur.RoleId equals role.Id into rGroup
                from r in rGroup.DefaultIfEmpty() // Позволяет выгрузить пользователя, даже если назначенная роль не найдена

                // 5. Проектируем промежуточный плоский результат, где строки могут дублироваться (Декартово произведение)
                select new { user, RoleName = r != null ? r.Name : null }
            )
                // 6. РЕШЕНИЕ ПРОБЛЕМЫ ДЕКАРТОВА ПРОИЗВЕДЕНИЯ: Группируем результат на стороне БД по уникальному объекту пользователя.
                // Это гарантирует, что база данных вернет строго ОДНУ строку на каждого человека, сколько бы ролей у него ни было.
                .GroupBy(x => x.user, x => x.RoleName)

                // 7. Формируем структуру ответа из сгруппированных данных на сервере БД с помощью маппера
                .Select(GetUsersListMappingExtensions.ToItemExpression)
                // 8. РЕШЕНИЕ ПРОБЛЕМЫ N+1: Выполняем весь собранный выше SQL-запрос за ОДИН асинхронный шаг.
                // Передаем токен отмены напрямую в драйвер базы данных, чтобы прервать операцию при отмене со стороны UI.
                .ToListAsync(cancellationToken);

            LogAllUsersSuccessfullyFetchedFromDb(logger, usersQuery.Count);

            // 9. Возвращаем единый объект ответа с вложенным списком пользователей
            return new GetUsersListResponse(usersQuery);
        }
    }
}
