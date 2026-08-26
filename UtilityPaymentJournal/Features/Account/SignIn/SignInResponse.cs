using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Account.SignIn
{
    /// <summary>
    /// ДТО ответа API, возвращающий результат выполнения команды аутентификации пользователя в системе.
    /// </summary>
    /// <param name="IsSuccess">Признак успешности прохождения аутентификации и создания сессии</param>
    /// <param name="Status">Бизнес-статус результата проверки учетных данных (Успех, Блокировка, Отказ)</param>
    /// <param name="ErrorMessage">Локализованное текстовое сообщение об ошибке для вывода пользователю в UI</param>
    public record SignInResponse
    (
        bool IsSuccess,
        SignInResultStatus Status,
        string? ErrorMessage = null)
    {
        public static SignInResponse Success()
            => new(true, SignInResultStatus.Success);

        public static SignInResponse LockedOut()
            => new(false, SignInResultStatus.LockedOut, "Аккаунт временно заблокирован из-за множества неудачных попыток входа.");

        public static SignInResponse NotAllowed()
            => new(false, SignInResultStatus.NotAllowed, "Доступ к системе ограничен. Обратитесь к администратору.");

        public static SignInResponse InvalidCredentials()
            => new(false, SignInResultStatus.InvalidCredentials, "Неверный логин или пароль.");
    }
}
