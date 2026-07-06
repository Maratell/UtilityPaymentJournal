using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Interface.Service
{
    /// <summary>
    /// Сервис управления процессами аутентификации и сессиями пользователей.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Выполняет аутентификацию пользователя в системе.
        /// </summary>
        Task<AuthenticationResultViewModel> SignInAsync(SignInRequestViewModel model);

        /// <summary>
        /// Завершает текущую сессию пользователя.
        /// </summary>
        Task SignOutAsync();
    }
}
