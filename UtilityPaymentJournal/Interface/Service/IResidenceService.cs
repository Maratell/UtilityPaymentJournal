using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.DTOs.UtilityProviders;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IResidenceService
    {
        Task<ResidenceDto> CreateAsync(CreateResidenceDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ResidenceDto> EditAsync(long id, EditResidenceDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ResidenceDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ResidenceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
