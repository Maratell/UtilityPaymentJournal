namespace UtilityPaymentJournal.Features.Residences.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных о показаниях счетчиков электроэнергии.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния БД в рамках паттерна CQRS.
    /// </summary>
    public interface IResidenceQueryService
    {
        /// <summary>
        /// Получить подробную информацию о показании счетчика электроэнергии по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи показания</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с развернутыми данными показания счетчика и текстовыми деталями связей</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если запись с указанным ID отсутствует в БД</exception>
        Task<ResidenceQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список всех зафиксированных показаний счетчиков электроэнергии в системе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ДТО показаний счетчиков электроэнергии, оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<ResidenceQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
