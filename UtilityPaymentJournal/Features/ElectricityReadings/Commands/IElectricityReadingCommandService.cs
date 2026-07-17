namespace UtilityPaymentJournal.Features.ElectricityReadings.Commands
{
    public interface IElectricityReadingCommandService
    {
        /// <summary>
        /// Создать новую запись показания счетчика электроэнергии.
        /// </summary>
        Task<ElectricityReadingCommandResultDto> CreateAsync(CreateElectricityReadingDto createDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновить существующую запись показания счетчика электроэнергии.
        /// </summary>
        Task<ElectricityReadingCommandResultDto> EditAsync(long id, EditElectricityReadingDto editDto, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удалить запись показания счетчика электроэнергии по идентификатору.
        /// </summary>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
