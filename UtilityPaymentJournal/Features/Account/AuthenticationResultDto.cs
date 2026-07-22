using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Account
{
    /// <summary>
    /// ДТО с результатом аутентификации пользователя (возвращается из бизнес-логики).
    /// </summary>
    /// <param name="IsSuccess">Флаг успешности проведения аутентификации</param>
    /// <param name="Status">Детализированный статус результата входа (например: Успешно, Заблокирован, Требуется 2FA)</param>
    /// <param name="ErrorMessage">Текст ошибки для отображения пользователю (заполняется только при IsSuccess = false)</param>
    public record AuthenticationResultDto(
        bool IsSuccess,
        SignInResultStatus Status,
        string? ErrorMessage = null
    );
}
