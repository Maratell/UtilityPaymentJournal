namespace UtilityPaymentJournal.Features.Account.GetCurrentUser
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию текущего аутентифицированного пользователя.
    /// </summary>
    /// <param name="Id">Уникальный строковый идентификатор пользователя в системе Identity</param>
    /// <param name="UserName">Уникальное имя пользователя (логин) для отображения в интерфейсе</param>
    /// <param name="FirstName">Имя пользователя для персонализации в интерфейсе (например, приветствие).</param>
    /// <param name="LastName">Фамилия пользователя.</param>
    /// <param name="Role">Основная роль пользователя в системе для разграничения прав на клиенте.</param>
    public record GetCurrentUserResponse(
        string Id,
        string? UserName, // сделал nullable, чтобы не тащить логику в маппер, поскольку в Identity это сво-во nullable
        string FirstName,
        string LastName,
        string? Role
    );
}
