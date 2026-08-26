namespace UtilityPaymentJournal.Features.Users.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одном пользователе в системе.
    /// </summary>
    /// /// <param name="Id">Уникальный строковый идентификатор пользователя (GUID)</param>
    /// <param name="UserName">Имя пользователя (логин) для входа в систему</param>
    /// <param name="FirstName">Имя пользователя, извлеченное из базы данных</param>
    /// <param name="LastName">Фамилия пользователя, извлеченная из базы данных</param>
    /// <param name="Role">Актуальное название роли пользователя</param>
    public record GetUserByIdResponse(
        string Id,
        string? UserName, // сделал nullable, чтобы не тащить логику в маппер, поскольку в Identity это св-во nullable
        string FirstName,
        string LastName,
        string? Role
    );
}
