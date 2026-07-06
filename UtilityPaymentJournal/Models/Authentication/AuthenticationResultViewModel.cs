using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Models.Authentication
{
    /// <summary>
    /// Результат выполнения операции аутентификации (входа/выхода).
    /// </summary>
    public class AuthenticationResultViewModel
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
