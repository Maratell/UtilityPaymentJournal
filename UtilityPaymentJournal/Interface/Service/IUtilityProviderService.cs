using UtilityPaymentJournal.DTOs.UtilityProviders;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityProviderService
    {
        Task<UtilityProviderDto> CreateAsync(CreateUtilityProviderDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<UtilityProviderDto> EditAsync(long id, EditUtilityProviderDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<UtilityProviderDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UtilityProviderDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
