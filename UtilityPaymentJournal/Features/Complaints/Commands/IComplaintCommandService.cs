namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления жалобами.
    /// Отвечает за операции создания, изменения, удаления данных и смену статусов (CUD) в рамках паттерна CQRS.
    /// </summary>
    public interface IComplaintCommandService
    {
        /// <summary>
        /// Создать новую запись жалобы в системе.
        /// </summary>
        /// <param name="createDto">ДТО с входными параметрами бизнес-логики для создания новой записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с результатом выполнения команды записи (содержит сгенерированный Id и плоские данные)</returns>
        Task<ComplaintCommandResultDto> CreateAsync(CreateComplaintDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую запись жалобы в системе.
        /// </summary>
        /// <param name="id">Уникальный идентификатор изменяемой записи в базе данных, полученный из маршрута URL</param>
        /// <param name="editDto">ДТО с новыми значениями полей для редактирования записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с обновленным результатом выполнения команды записи для подтверждения успешности операции</returns>
        Task<ComplaintCommandResultDto> EditAsync(long id, EditComplaintDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Точечно изменить статус существующей жалобы.
        /// </summary>
        /// <param name="changeStatusDto">ДТО с идентификатором жалобы и новым целевым статусом</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с обновленным результатом выполнения команды записи после смены статуса</returns>
        Task<ComplaintCommandResultDto> ChangeStatusAsync(ChangeComplaintStatusDto changeStatusDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить запись жалобы из системы по её идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемой записи в базе данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Значение true, если запись была успешно найдена и удалена; иначе false (например, если запись отсутствует в системе)</returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
