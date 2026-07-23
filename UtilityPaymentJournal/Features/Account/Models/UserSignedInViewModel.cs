using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Account.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции входа в систему (ответ на POST).
    /// </summary>
    public class UserSignedInViewModel
    {
        /// <summary>
        /// Флаг успешности выполнения операции.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Статус результата попытки входа пользователя в систему.
        /// </summary>
        public SignInResultStatus Status { get; set; }
        /// <summary>
        /// Сообщение об ошибке (заполняется, если IsSuccess = false).
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// URL для перенаправления пользователя после успешного действия.
        /// </summary>
        public string? RedirectUrl { get; set; }
    }
}
