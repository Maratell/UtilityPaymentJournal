using UtilityPaymentJournal.DTOs.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IComplaintService
    {
        Task<ComplaintDTO> CreateAsync(CreateComplaintDTO createDto, CancellationToken cancellationToken = default);
        Task<ComplaintDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ComplaintDTO?> EditAsync(long id, EditComplaintDTO editDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ComplaintDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
