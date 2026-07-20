using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string IconClass { get; set; } = string.Empty;

        /// <summary>
        /// Статус активности коммунальной услуги. Визуально реализует паттерн "Мягкое удаление" (Soft Delete):
        /// true — активна (доступна для выбора), false — деактивирована (архивная / мягко удаленная запись).
        /// Вместо физического удаления строки (DELETE) флаг сбрасывается в false для сохранения целостности исторических данных.
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true; // По умолчанию услуга активна

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
