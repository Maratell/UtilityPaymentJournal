using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.DTO.UtilityProviders;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IResidenceService
    {
        Task<ResidenceDTO> CreateAsync(CreateResidenceDTO dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ResidenceDTO?> EditAsync(long id, EditResidenceDTO dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ResidenceDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ResidenceDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
