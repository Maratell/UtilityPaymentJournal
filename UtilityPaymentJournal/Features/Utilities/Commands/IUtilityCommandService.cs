namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления коммунальными услугами.
    /// Отвечает за операции создания, изменения и удаления данных (CUD) в рамках паттерна CQRS.
    /// </summary>
    public interface IUtilityCommandService
    {
        /// <summary>
        /// Создать новую коммунальную услугу в системе.
        /// </summary>
        /// <param name="createDto">ДТО с входными параметрами бизнес-логики для создания новой услуги</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с результатом выполнения команды записи (содержит сгенерированный Id и плоские данные)</returns>
        Task<UtilityCommandResultDto> CreateAsync(CreateUtilityDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую коммунальную услугу в системе.
        /// </summary>
        /// <param name="id">Уникальный идентификатор изменяемой услуги в базе данных, полученный из маршрута URL</param>
        /// <param name="editDto">ДТО с новыми значениями полей для редактирования услуги</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с обновленным результатом выполнения команды записи для подтверждения успешности операции</returns>
        Task<UtilityCommandResultDto> EditAsync(long id, EditUtilityDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить коммунальную услугу по её идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемой услуги в базе данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Значение true, если услуга была успешно найдена и удалена; иначе false (например, если запись отсутствует в системе)</returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
