using UtilityPaymentJournal.DTOs.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IComplaintService
    {
        Task<ComplaintDto> CreateAsync(CreateComplaintDto createDto, CancellationToken cancellationToken = default);
        Task<ComplaintDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ComplaintDto> EditAsync(long id, EditComplaintDto editDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ComplaintDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
