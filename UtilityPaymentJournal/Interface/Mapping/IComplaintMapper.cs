using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IComplaintMapper
    {
        CreateComplaintDto ToDto(CreateComplaintViewModel vm);
        ComplaintDto ToDto(Complaint entity);
        EditComplaintDto ToDto(EditComplaintViewModel vm);
        EditComplaintDto ToDto(ComplaintDto dto, ComplaintStatus status);
        Complaint ToEntity(CreateComplaintDto dto);
        ComplaintViewModel ToViewModel(ComplaintDto dto);
        void UpdateEntity(EditComplaintDto dto, Complaint entity);
    }
}
