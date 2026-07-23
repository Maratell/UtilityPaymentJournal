using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Account.Commands
{
    /// <summary>
    /// ДТО результата выполнения команды аутентификации пользователя в системе.
    /// Используется для возврата статуса операции из бизнес-логики в контроллер (SignIn).
    /// </summary>
    /// <param name="IsSuccess">Признак успешности прохождения аутентификации и создания сессии</param>
    /// <param name="Status">Бизнес-статус результата проверки учетных данных (Успех, Блокировка, Отказ)</param>
    /// <param name="ErrorMessage">Локализованное текстовое сообщение об ошибке для вывода пользователю в UI</param>
    public record AuthenticationCommandResultDto(
        bool IsSuccess,
        SignInResultStatus Status,
        string? ErrorMessage = null
    );
}
