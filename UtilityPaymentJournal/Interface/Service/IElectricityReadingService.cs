using UtilityPaymentJournal.DTOs.ElectricityReadings;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IElectricityReadingService
    {
        Task<ElectricityReadingDto> CreateAsync(CreateElectricityReadingDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ElectricityReadingDto?> EditAsync(long id, EditElectricityReadingDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ElectricityReadingDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ElectricityReadingDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
