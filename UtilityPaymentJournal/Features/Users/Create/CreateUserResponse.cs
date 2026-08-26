namespace UtilityPaymentJournal.Features.Users.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданного пользователя в БД.
    /// </summary>
    /// <param name="Id">Уникальный строковый идентификатор (GUID) созданного пользователя, сгенерированный базой данных</param>
    /// <param name="UserName">Уникальное имя пользователя (логин), под которым он был зарегистрирован</param>
    /// <param name="FirstName">Имя созданного пользователя</param>
    /// <param name="LastName">Фамилия созданного пользователя</param>
    /// <param name="Role">Название системной роли, которая была успешно присвоена пользователю</param>
    public record CreateUserResponse
    (
        string Id,
        string? UserName, // сделал nullable, чтобы не тащить логику в маппер, поскольку в Identity это св-во nullable
        string FirstName,
        string LastName,
        string? Role
    );
}
