using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Users.Create
{
    /// <summary>
    /// Запрос на создание новой учетной записи пользователя в системе.
    /// </summary>
    /// <param name="UserName">Уникальное имя пользователя (логин) для входа в систему</param>
    /// <param name="FirstName">Имя пользователя для отображения в профиле</param>
    /// <param name="LastName">Фамилия пользователя для отображения в профиле</param>
    /// <param name="Password">Пароль учетной записи, соответствующий системным политикам безопасности</param>
    /// <param name="Role">Системная роль, которая будет автоматически присвоена пользователю при создании</param>
    public record CreateUserRequest(
        string UserName,
        string FirstName,
        string LastName,
        string Password,
        UserRole Role
    )
    {
        public CreateUserCommand ToCommand() =>
            new(
                UserName,
                FirstName,
                LastName,
                Password,
                Role
            );
    }
}
