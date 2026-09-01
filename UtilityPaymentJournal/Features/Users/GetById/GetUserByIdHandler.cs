using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.GetById
{
    /// <summary>
    /// Обработчик запроса на получение детальной информации о пользователе в системе.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetUserByIdHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<GetUserByIdHandler> logger) : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingUserById(logger, query.Id);

            // Ищем пользователя по его ID встроенным методом Identity
            User? user = await userManager.FindByIdAsync(query.Id);
            if (user is null)
            {
                LogUserNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Учетная запись пользователя с идентификатором {query.Id} не найдена в БД.");
            }

            // Получаем роли пользователя и берем первую из них. 
            IList<string> roles = await userManager.GetRolesAsync(user);
            string? role = roles.FirstOrDefault(); 

            LogUserSuccessfullyFetchedFromDb(logger, query.Id, role ?? string.Empty);
            return user.ToResponse(role);
        }


        #region ОПТИМИЗИРОВАННЫЙ ЗАПРОС (РЕШИЛ ЗАКОММЕНТИРОВАТЬ ДЛЯ ДЕМОНСТРАЦИИ БОЛЕЕ ЛАКОНИЧНОЙ ЗАПИСИ ЧЕРЕЗ IDENTITY)

        //public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        //{
        //    LogFetchingUserById(logger, query.Id);
        //    // ОПТИМИЗАЦИЯ ЗАПРОСА (CQRS Чтение):
        //    // 1. Избегаем двух лишних сетевых задержек: Стандартный UserManager потребовал бы два раздельных 
        //    //    запроса (FindByIdAsync + GetRolesAsync). Мы пишем один лаконичный LINQ-запрос с явными 
        //    //    LEFT JOIN, заставляя базу данных соединить таблицы AspNetUsers, AspNetUserRoles и AspNetRoles 
        //    //    на своей стороне и вернуть весь результат за ОДИН единственный сетевой round-trip.
        //    // 2. Отключаем кэш слежения (.AsNoTracking()): Поскольку это операция чтения (Query), нам не нужно 
        //    //    изменять состояние сущности. Мы говорим EF Core не тратить оперативную память и ресурсы CPU 
        //    //    на создание тяжелых трекеров изменений (Change Tracker), что существенно ускоряет выполнение.
        //    var userData = await (
        //        from user in context.Users.AsNoTracking()
        //        where user.Id == query.Id

        //        // Делаем первый LEFT JOIN со связующей таблицей, чтобы не потерять пользователя, если у него нет роли
        //        join userRole in context.UserRoles on user.Id equals userRole.UserId into urGroup
        //        from ur in urGroup.DefaultIfEmpty()

        //            // Делаем второй LEFT JOIN с таблицей ролей для получения текстового названия роли
        //        join role in context.Roles on ur.RoleId equals role.Id into rGroup
        //        from r in rGroup.DefaultIfEmpty()

        //            // Формируем  легкий анонимный объект, вытягивая сущность и вычисленное имя роли
        //        select new { user, RoleName = r != null ? r.Name : null }
        //    ).FirstOrDefaultAsync(cancellationToken);

        //    if (userData is null)
        //    {
        //        LogUserNotFoundInDb(logger, query.Id);
        //        throw new KeyNotFoundException($"Учетная запись пользователя с идентификатором {query.Id} не найдена в БД.");
        //    }

        //    LogUserSuccessfullyFetchedFromDb(logger, query.Id, userData.RoleName);
        //    return userData.user.ToResponse(userData.RoleName);
        //}

        #endregion
    }
}
