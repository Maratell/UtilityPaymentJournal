using System.Linq.Expressions;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Users.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка пользователей в системе.
    /// </summary>
    public static class GetUsersListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="User"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetUsersListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetUsersListResponse ToResponse(this IEnumerable<(User User, string? RoleName)> data)
        {
            ArgumentNullException.ThrowIfNull(data);

            // Трансформируем сущности во вложенные рекорды Item
            GetUsersListResponse.Item[] items = data
                .Select(e => new GetUsersListResponse.Item(
                    Id: e.User.Id,
                    UserName: e.User.UserName, 
                    FirstName: e.User.FirstName,
                    LastName: e.User.LastName,
                    Role: e.RoleName           
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetUsersListResponse(items);
        }

        public static Expression<Func<IGrouping<User, string?>, GetUsersListResponse.Item>> ToItemExpression =>
            group => new GetUsersListResponse.Item(
                group.Key.Id,
                group.Key.UserName,
                group.Key.FirstName,
                group.Key.LastName,
                group.Where(name => name != null).FirstOrDefault()
            );
    }
}
