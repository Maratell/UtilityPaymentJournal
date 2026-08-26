
namespace UtilityPaymentJournal.Features.Account.SignIn
{
    /// <summary>
    /// Запрос на вход пользователя в систему (аутентификация)
    /// </summary>
    /// <param name="UserName">Имя пользователя для входа</param>
    /// <param name="Password">Пароль пользователя</param>
    /// <param name="IsPersistent">Флаг "Запомнить меня" (сохранять ли сессию/куки после закрытия браузера)</param>
    public record SignInRequest(
        string UserName,
        string Password,
        bool IsPersistent
    )
    {
        public SignInCommand ToCommand() =>
            new(
                UserName, 
                Password, 
                IsPersistent
            );
    }
}
