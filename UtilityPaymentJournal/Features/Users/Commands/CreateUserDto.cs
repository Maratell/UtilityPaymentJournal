using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Users.Commands
{
    /// <summary>
    /// ДТО для создания новой учетной записи пользователя в системе.
    /// Передается клиентом в теле запроса (API Request) при выполнении команды регистрации.
    /// </summary>
    /// <param name="UserName">Уникальное имя пользователя (логин) для входа в систему</param>
    /// <param name="FirstName">Имя пользователя для отображения в профиле</param>
    /// <param name="LastName">Фамилия пользователя для отображения в профиле</param>
    /// <param name="Password">Пароль учетной записи, соответствующий системным политикам безопасности</param>
    /// <param name="Role">Системная роль, которая будет автоматически присвоена пользователю при создании</param>
    public record CreateUserDto(
        string UserName,
        string FirstName,
        string LastName,
        string Password,
        UserRole Role
    );
}
