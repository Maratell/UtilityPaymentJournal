using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.Admin
{
    /// <summary>
    /// ДТО для самостоятельной регистрации нового пользователя в системе.
    /// </summary>
    /// <param name="UserName">Уникальное имя пользователя (логин) для входа</param>
    /// <param name="FirstName">Имя пользователя</param>
    /// <param name="LastName">Фамилия пользователя</param>
    /// <param name="Password">Пароль для новой учетной записи</param>
    /// <param name="Role">Системная роль (заложена на вырост, сейчас по умолчанию всегда передается User)</param>
    public record CreateUserDto(
        string UserName,
        string FirstName,
        string LastName,
        string Password,
        UserRole Role
    );
}
