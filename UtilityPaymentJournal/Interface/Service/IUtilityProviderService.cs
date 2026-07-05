using UtilityPaymentJournal.DTO.UtilityProviders;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityProviderService
    {
        Task<UtilityProviderDTO> CreateAsync(CreateUtilityProviderDTO dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<UtilityProviderDTO?> EditAsync(long id, EditUtilityProviderDTO dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<UtilityProviderDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UtilityProviderDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
