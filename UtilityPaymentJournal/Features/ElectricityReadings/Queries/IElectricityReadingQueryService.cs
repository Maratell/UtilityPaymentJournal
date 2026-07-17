namespace UtilityPaymentJournal.Features.ElectricityReadings.Queries
{
    public interface IElectricityReadingQueryService
    {
        /// <summary>
        /// Получить подробную информацию о показании счетчика электроэнергии по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи показания</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с развернутыми данными показания счетчика и текстовыми деталями связей</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если запись с указанным ID отсутствует в БД</exception>
        Task<ElectricityReadingQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список всех зафиксированных показаний счетчиков электроэнергии в системе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ДТО показаний счетчиков электроэнергии, оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<ElectricityReadingQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
