using UtilityPaymentJournal.DTOs.ElectricityReadings;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IElectricityReadingService
    {
        Task<ElectricityReadingDTO> CreateAsync(CreateElectricityReadingDTO dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ElectricityReadingDTO?> EditAsync(long id, EditElectricityReadingDTO dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ElectricityReadingDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ElectricityReadingDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
