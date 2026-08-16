using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities
{
    public class UtilityProvider : IAuditable, IUserOwned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // Внешний ключ для связи с пользователем
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Наименование поставщика услуг обязательно для заполнения")]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<UtilityProviderLink> Links { get; set; } = new List<UtilityProviderLink>();
    }
}
