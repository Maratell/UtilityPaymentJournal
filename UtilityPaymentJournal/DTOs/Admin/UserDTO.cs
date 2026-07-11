namespace UtilityPaymentJournal.DTOs.Admin
{
    /// <summary>
    /// ДТО для возврата данных о пользователе (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор пользователя в системе (GUID)</param>
    /// <param name="UserName">Имя пользователя (логин) для входа</param>
    /// <param name="FirstName">Имя пользователя</param>
    /// <param name="LastName">Фамилия пользователя</param>
    /// <param name="RoleName">Строковое название системной роли пользователя (например: Admin, User)</param>
    public record UserDto(
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string RoleName
    );
}
