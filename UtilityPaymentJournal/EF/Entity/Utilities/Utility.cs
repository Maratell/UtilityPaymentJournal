
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.EF.Entity.Utilities
{
    public class Utility : IAuditable/*, IUserOwned*/
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id { get; set; }

        //// Внешний ключ для связи с пользователем
        //[Required]
        //public string UserId { get; set; } = string.Empty;

        //[ForeignKey("UserId")]
        //public User User { get; set; } = null!;

        [Required(ErrorMessage = "Наименование услуги обязательно для заполнения")]
        public string Name { get; set; } = string.Empty;

        // Поле для хранения класса иконки Bootstrap Icons
        [Required]
        [MaxLength(50)]
        public string IconClass { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
