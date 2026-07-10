using UtilityPaymentJournal.DTOs.Account;

namespace UtilityPaymentJournal.Interface.Service
{
    /// <summary>
    /// Интерфейс сервиса аутентификации пользователей.
    /// Определяет контракты для управления сессиями входа и выхода из системы.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Проверяет учетные данные пользователя и создает сессию аутентификации.
        /// </summary>
        /// <param name="signInDto">DTO данных для входа (логин, пароль, флаг постоянной сессии).</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Результат аутентификации в виде <see cref="AuthenticationResultDTO"/>.</returns>
        Task<AuthenticationResultDTO> SignInAsync(SignInDto signInDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Завершает текущую активную сессию пользователя (выход из системы).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task SignOutAsync(CancellationToken cancellationToken = default);
    }
}
