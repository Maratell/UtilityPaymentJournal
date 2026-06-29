using UtilityPaymentJournal.DTO.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IComplaintBoardService
    {
        public Task<ComplaintDTO> CreateAsync(CreateComplaintDTO dto);
        public Task DeleteAsync(long id);
        public Task<ComplaintDTO> EditAsync(long id, EditComplaintDTO dto);
        public Task<IEnumerable<ComplaintDTO>> GetAllAsync();
    }
}
