
namespace UtilityPaymentJournal.Features.Users.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления пользователями.
    /// Отвечает исключительно за операции, изменяющие состояние системы (W) в рамках паттерна CQRS.
    /// </summary>
    public interface IUserCommandService
    {
        /// <summary>
        /// Транзакционно создать нового пользователя в системе и привязать к нему системную роль.
        /// </summary>
        /// <param name="createDto">ДТО с входными данными для создания учетной записи пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО созданного пользователя с заполненным системным идентификатором</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект DTO равен null</exception>
        /// Выбрасывается при нарушении правил валидации пароля или уникальности данных Identity</exception>
        Task<UserCommandResultDto> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Безвозвратно удалить учетную запись пользователя из системы по его идентификатору.
        /// </summary>
        /// <param name="id">Уникальный строковый идентификатор удаляемого пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Значение true, если пользователь успешно удален; иначе — false (например, если пользователь не найден)</returns>
        /// Выбрасывается, если менеджер Identity заблокировал операцию удаления из-за системных ограничений</exception>
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
