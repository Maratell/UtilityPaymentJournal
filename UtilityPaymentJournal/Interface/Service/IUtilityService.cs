using UtilityPaymentJournal.DTOs.Utilities;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityService
    {
        Task<UtilityDto> CreateAsync(CreateUtilityDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<UtilityDto> EditAsync(long id, EditUtilityDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<UtilityDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UtilityDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
