using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Utilities.Models
{
    /// <summary>
    /// Модель представления для редактирования существующей коммунальной услуги в интерфейсе.
    /// </summary>
    public class EditUtilityViewModel
    {
        /// <summary>
        /// Новое наименование коммунальной услуги.
        /// </summary>
        [Display(Name = "Наименование услуги")]
        [Required(ErrorMessage = "Пожалуйста, введите наименование услуги")]
        [StringLength(100, ErrorMessage = "Наименование не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Новое значение класса иконки Bootstrap Icons.
        /// </summary>
        [Display(Name = "Класс иконки")]
        [Required(ErrorMessage = "Пожалуйста, укажите класс иконки")]
        [MaxLength(50, ErrorMessage = "Длина класса иконки не должна превышать 50 символов")]
        public string IconClass { get; set; } = string.Empty;
        /// <summary>
        /// Новый статус активности коммунальной услуги.
        /// </summary>
        [Display(Name = "Статус активности")]
        [Required]
        public bool IsActive { get; set; }
    }
}
