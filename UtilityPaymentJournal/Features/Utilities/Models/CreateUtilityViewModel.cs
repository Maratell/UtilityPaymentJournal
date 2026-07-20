using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Utilities.Models
{
    /// <summary>
    /// Модель представления для создания новой коммунальной услуги.
    /// </summary>
    public class CreateUtilityViewModel
    {
        /// <summary>
        /// Наименование коммунальной услуги (например, Водоснабжение, Электроэнергия,...).
        /// </summary>
        [Display(Name = "Наименование услуги")]
        [Required(ErrorMessage = "Пожалуйста, введите наименование услуги")]
        [StringLength(100, ErrorMessage = "Наименование не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Класс иконки Bootstrap Icons для визуализации услуги в интерфейсе.
        /// </summary>
        [Display(Name = "Класс иконки")]
        [Required(ErrorMessage = "Пожалуйста, выберите или введите класс иконки")]
        [MaxLength(50, ErrorMessage = "Длина класса иконки не должна превышать 50 символов")]
        public string IconClass { get; set; } = string.Empty;
        /// <summary>
        /// Статус активности коммунальной услуги в системе.
        /// </summary>
        [Display(Name = "Активна")]
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
