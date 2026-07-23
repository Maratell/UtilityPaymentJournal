namespace UtilityPaymentJournal.Features.Account.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции выхода из системы (ответ на POST).
    /// </summary>
    public class UserSignedOutViewModel
    {
        /// <summary>
        /// Флаг успешности выполнения операции.
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        /// <summary>
        /// URL для перенаправления пользователя после успешного действия.
        /// </summary>
        public string? RedirectUrl { get; set; }
    }
}
