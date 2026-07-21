using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Residences.Models
{
    /// <summary>
    /// Модель представления для редактирования существующего объекта недвижимости.
    /// </summary>
    public class EditResidenceViewModel
    {
        /// <summary>
        /// Новый полный адрес объекта недвижимости (город, улица, номер дома, квартира).
        /// </summary>
        [Display(Name = "Адрес объекта")]
        [Required(ErrorMessage = "Пожалуйста, введите адрес объекта недвижимости")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Адрес должен содержать от 5 до 100 символов")]
        public string Address { get; set; } = string.Empty;
    }
}
