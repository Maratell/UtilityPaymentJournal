namespace UtilityPaymentJournal.Features.Account.Commands
{
    /// <summary>
    /// ДТО с данными пользователя для входа в систему (аутентификация).
    /// </summary>
    /// <param name="UserName">Имя пользователя для входа</param>
    /// <param name="Password">Пароль пользователя</param>
    /// <param name="IsPersistent">Флаг "Запомнить меня" (сохранять ли сессию/куки после закрытия браузера)</param>
    public record SignInDto(
        string UserName,
        string Password,
        bool IsPersistent
    );
}
