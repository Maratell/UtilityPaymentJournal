using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Infrastructure.EF.Entity.Residences
{
    public class Residence : IAuditable, IUserOwned
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id { get; set; }

        [Required(ErrorMessage = "Адрес обязателен для заполнения")] 
        public string Address { get; set; } = string.Empty;

        // Внешний ключ для связи с пользователем
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
