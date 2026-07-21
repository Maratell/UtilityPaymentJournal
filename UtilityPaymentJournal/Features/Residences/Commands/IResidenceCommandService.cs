namespace UtilityPaymentJournal.Features.Residences.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления объектами недвижимости.
    /// Отвечает за операции создания, изменения и удаления данных (CUD) в рамках паттерна CQRS.
    /// </summary>
    public interface IResidenceCommandService
    {
        /// <summary>
        /// Создать новую запись объекта недвижимости.
        /// </summary>
        /// <param name="createDto">ДТО с входными параметрами бизнес-логики для создания новой записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с результатом выполнения команды записи (содержит сгенерированный Id и плоские данные)</returns>
        Task<ResidenceCommandResultDto> CreateAsync(CreateResidenceDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую запись объекта недвижимости.
        /// </summary>
        /// <param name="id">Уникальный идентификатор изменяемой записи в базе данных, полученный из маршрута URL</param>
        /// <param name="editDto">ДТО с новыми значениями полей для редактирования записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с обновленным результатом выполнения команды записи для подтверждения успешности операции</returns>
        Task<ResidenceCommandResultDto> EditAsync(long id, EditResidenceDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить запись объекта недвижимости по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемой записи в базе данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Значение true, если запись была успешно найдена и удалена; иначе false (например, если запись отсутствует в системе)</returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
