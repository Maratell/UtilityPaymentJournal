namespace UtilityPaymentJournal.Features.WaterReadings.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления показаниями счетчиков воды.
    /// Отвечает за операции создания, изменения и удаления данных (CUD) в рамках паттерна CQRS.
    /// </summary>
    public interface IWaterReadingCommandService
    {
        /// <summary>
        /// Создать новую запись показания счетчика воды.
        /// </summary>
        /// <param name="createDto">ДТО с входными данными для создания записи</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с результатом выполнения команды записи</returns>
        Task<WaterReadingCommandResultDto> CreateAsync(CreateWaterReadingDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую запись показания счетчика воды.
        /// </summary>
        /// <param name="id">Уникальный идентификатор изменяемой записи в БД</param>
        /// <param name="editDto">ДТО с новыми данными для редактирования</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с результатом выполнения команды записи</returns>
        Task<WaterReadingCommandResultDto> EditAsync(long id, EditWaterReadingDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить запись показания счетчика воды по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемой записи в БД</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True, если запись успешно удалена; иначе false (например, если запись не найдена)</returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
