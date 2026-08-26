using MediatR;

namespace UtilityPaymentJournal.Features.Account.SignIn
{
    /// <summary>
    /// Команда на вход пользователя в систему (аутентификация)
    /// </summary>
    /// <param name="UserName">Имя пользователя для входа</param>
    /// <param name="Password">Пароль пользователя</param>
    /// <param name="IsPersistent">Флаг "Запомнить меня" (сохранять ли сессию/куки после закрытия браузера)</param>
    public record SignInCommand(
        string UserName,
        string Password,
        bool IsPersistent
    ) : IRequest<SignInResponse>;
}
