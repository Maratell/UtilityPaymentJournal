namespace UtilityPaymentJournal.Features.UtilityProviders.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных о поставщиках коммунальных услуг.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния БД в рамках паттерна CQRS.
    /// </summary>
    public interface IUtilityProviderQueryService
    {
        /// <summary>
        /// Получить подробную информацию о поставщике коммунальных услуг по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи поставщика услуг</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с развернутыми данными поставщика услуг и текстовыми деталями связей</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если запись с указанным ID отсутствует в БД</exception>
        Task<UtilityProviderQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список всех зафиксированных поставщиков коммунальных услуг в системе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ДТО поставщиков коммунальных услуг, оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<UtilityProviderQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
