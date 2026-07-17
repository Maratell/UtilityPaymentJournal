namespace UtilityPaymentJournal.Features.WaterReadings.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных о показаниях счетчиков воды.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния БД в рамках паттерна CQRS.
    /// </summary>
    public interface IWaterReadingQueryService
    {
        /// <summary>
        /// Получить подробную информацию о показании счетчика воды по его идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор записи показания в базе данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с развернутыми данными показания счетчика воды и подтянутыми текстовыми деталями связей</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если запись с указанным ID отсутствует в базе данных</exception>
        Task<WaterReadingQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список всех зафиксированных показаний счетчиков воды в системе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Коллекция ДТО показаний счетчиков воды, полностью оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<WaterReadingQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
