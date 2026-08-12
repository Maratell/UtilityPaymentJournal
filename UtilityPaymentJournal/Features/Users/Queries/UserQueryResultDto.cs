namespace UtilityPaymentJournal.Features.Users.Queries
{
    /// <summary>
    /// ДТО результата запроса данных пользователя.
    /// Отвечает исключительно за предоставление данных (R) и полностью оптимизирован для вывода на UI (GetById/GetAll).
    /// </summary>
    /// <param name="Id">Уникальный системный идентификатор пользователя (GUID)</param>
    /// <param name="UserName">Имя пользователя (логин) для входа в систему</param>
    /// <param name="FirstName">Имя пользователя, извлеченное из базы данных</param>
    /// <param name="LastName">Фамилия пользователя, извлеченная из базы данных</param>
    /// <param name="RoleName">Актуальное название текстовой роли пользователя (например: Admin, User, Manager)</param>
    public record UserQueryResultDto(
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string RoleName
    );
}
