using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTO.ComplaintBoard
{
    public class EditComplaintDTO
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public long UtilityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SubmissionDate { get; set; }

        public DateTime? IssueResolutionDate { get; set; }

        public ComplaintStatus Status { get; set; }
    }
}
