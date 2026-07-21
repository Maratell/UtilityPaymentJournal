namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    /// <summary>
    /// Интерфейс сервиса команд (записи) для управления поставщиками коммунальных услуг.
    /// Отвечает за операции создания, изменения и удаления данных (CUD) в рамках паттерна CQRS.
    /// </summary>
    public interface IUtilityProviderCommandService
    {
        /// <summary>
        /// Создать новую запись поставщика коммунальных услуг.
        /// </summary>
        /// <param name="createDto">ДТО с входными параметрами бизнес-логики для создания новой записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с результатом выполнения команды записи (содержит сгенерированный Id и плоские данные)</returns>
        Task<UtilityProviderCommandResultDto> CreateAsync(CreateUtilityProviderDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую запись поставщика коммунальных услуг.
        /// </summary>
        /// <param name="id">Уникальный идентификатор изменяемой записи в базе данных, полученный из маршрута URL</param>
        /// <param name="editDto">ДТО с новыми значениями полей для редактирования записи</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с обновленным результатом выполнения команды записи для подтверждения успешности операции</returns>
        Task<UtilityProviderCommandResultDto> EditAsync(long id, EditUtilityProviderDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить запись поставщика коммунальных услуг по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемой записи в базе данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Значение true, если запись была успешно найдена и удалена; иначе false (например, если запись отсутствует в системе)</returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
