using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Enumerations;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.EF.Entity.ComplaintBoard
{
    public class Complaint : IAuditable, IUserOwned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // Внешний ключ для связи с пользователем
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Услуга обязательна для заполнения")]
        public long UtilityId { get; set; }

        [ForeignKey(nameof(UtilityId))]
        public Utility Utility { get; set; } = null!; // У одной жалобы может быть одна услуга

        public DateTime? SubmissionDate { get; set; }

        public DateTime? IssueResolutionDate { get; set; }

        [Required]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
