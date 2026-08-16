using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities
{
    /// <summary>
    /// Промежуточная таблица связей многие ко многим для услуг и поставщиков услуг
    /// </summary>
    public class UtilityProviderLink : IAuditable, IUserOwned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Id поставщика услуг обязателен для заполнения")]
        public long UtilityProviderId { get; set; }

        [ForeignKey(nameof(UtilityProviderId))] 
        public UtilityProvider UtilityProvider { get; set; } = null!;

        [Required(ErrorMessage = "Id услуги обязателен для заполнения")]
        public long UtilityId { get; set; }

        [ForeignKey(nameof(UtilityId))]
        public Utility Utility { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
