using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.Account
{
    // Результат аутентификации из бизнес-логики (без инфраструктурных Url)
    public class AuthenticationResultDTO
    {
        public bool IsSuccess { get; set; }
        public SignInResultStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
