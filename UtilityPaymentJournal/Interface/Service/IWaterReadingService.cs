using UtilityPaymentJournal.DTOs.WaterReadings;

namespace WaterReadingPaymentJournal.Interface.Service
{
    public interface IWaterReadingService
    {
        Task<WaterReadingDto> CreateAsync(CreateWaterReadingDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<WaterReadingDto?> EditAsync(long id, EditWaterReadingDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<WaterReadingDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<WaterReadingDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
