

namespace UtilityPaymentJournal.Features.Users.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) данных пользователей.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния пользователей или БД в рамках паттерна CQRS.
    /// </summary>
    public interface IUserQueryService
    {
        /// <summary>
        /// Получить развернутые оптимизированные данные пользователя по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">Строковый идентификатор запрашиваемого пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с детальными данными пользователя или null, если учетная запись с таким ID отсутствует</returns>
        Task<UserQueryResultDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить полный список всех зарегистрированных пользователей системы за один эффективный запрос.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ДТО с данными пользователей, оптимизированная для вывода в списках</returns>
        Task<IReadOnlyCollection<UserQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
