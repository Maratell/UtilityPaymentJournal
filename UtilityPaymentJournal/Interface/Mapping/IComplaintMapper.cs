using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTO.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IComplaintMapper
    {
        CreateComplaintDTO ToDto(CreateComplaintViewModel vm);
        ComplaintDTO ToDto(Complaint entity);
        EditComplaintDTO ToDto(EditComplaintViewModel vm);
        EditComplaintDTO ToDto(ComplaintDTO dto, ComplaintStatus status);
        Complaint ToEntity(CreateComplaintDTO dto);
        ComplaintViewModel ToViewModel(ComplaintDTO dto);
        void UpdateEntity(EditComplaintDTO dto, Complaint entity);
    }
}
