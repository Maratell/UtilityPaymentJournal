using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IComplaintMapper
    {
        CreateComplaintDto ToDto(CreateComplaintViewModel createViewModel);
        ComplaintDto ToDto(Complaint entity);
        EditComplaintDto ToDto(EditComplaintViewModel editViewModel);
        EditComplaintDto ToDto(ComplaintDto dto, ComplaintStatus status);
        Complaint ToEntity(CreateComplaintDto createDto);
        ComplaintViewModel ToViewModel(ComplaintDto dto);
        void UpdateEntity(EditComplaintDto editDto, Complaint entity);
    }
}
