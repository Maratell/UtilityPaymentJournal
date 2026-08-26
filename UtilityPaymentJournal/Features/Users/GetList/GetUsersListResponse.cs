
namespace UtilityPaymentJournal.Features.Users.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка пользователей в системе.
    /// </summary>
    public record GetUsersListResponse(IReadOnlyCollection<GetUsersListResponse.Item> Items)
    {
        public record Item(
            string Id,
            string? UserName, // сделал nullable, чтобы не тащить логику в маппер, поскольку в Identity это св-во nullable
            string FirstName,
            string LastName,
            string? Role
        );
    }
}
