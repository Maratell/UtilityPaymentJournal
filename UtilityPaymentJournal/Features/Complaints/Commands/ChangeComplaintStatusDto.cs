using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// ДТО бизнес-логики для изменения статуса жалобы.
    /// </summary>
    public record ChangeComplaintStatusDto(
        long Id,
        ComplaintStatus NewStatus
    );
}
