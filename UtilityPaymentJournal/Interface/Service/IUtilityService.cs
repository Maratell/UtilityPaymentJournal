using UtilityPaymentJournal.DTO.Utilities;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityService
    {
        Task<UtilityDTO> CreateAsync(CreateUtilityDTO dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<UtilityDTO?> EditAsync(long id, EditUtilityDTO dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<UtilityDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UtilityDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
