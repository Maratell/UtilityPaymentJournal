using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.ComplaintBoard
{
    public class ComplaintDTO
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public long UtilityId { get; set; }

        public string? UtilityName { get; set; } 

        public string? UtilityIcon { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SubmissionDate { get; set; }

        public DateTime? IssueResolutionDate { get; set; }

        public ComplaintStatus Status { get; set; }
    }
}
