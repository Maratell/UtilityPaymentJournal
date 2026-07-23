namespace UtilityPaymentJournal.Features.Account.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления сессиями аутентификации.
    /// Отвечает исключительно за операции, изменяющие состояние системы (W) в рамках паттерна CQRS.
    /// </summary>
    public interface IAuthenticationCommandService
    {
        /// <summary>
        /// Проверить подлинность учетных данных пользователя и создать активную сессию в системе.
        /// </summary>
        /// <param name="signInDto">ДТО с входными учетными данными пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО результата выполнения команды со статусом операции и сообщением об ошибке</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект DTO равен null</exception>
        Task<AuthenticationCommandResultDto> SignInAsync(SignInDto signInDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Асинхронно завершить текущую активную сессию пользователя в приложении с удалением кук.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию выхода</returns>
        Task SignOutAsync(CancellationToken cancellationToken = default);
    }
}
