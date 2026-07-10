using UtilityPaymentJournal.DTOs.WaterReadings;

namespace WaterReadingPaymentJournal.Interface.Service
{
    public interface IWaterReadingService
    {
        Task<WaterReadingDTO> CreateAsync(CreateWaterReadingDTO dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<WaterReadingDTO?> EditAsync(long id, EditWaterReadingDTO dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<WaterReadingDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<WaterReadingDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
