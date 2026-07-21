using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.UtilityProviders.Models
{
    /// <summary>
    /// Модель представления для создания нового поставщика коммунальных услуг.
    /// </summary>
    public class CreateUtilityProviderViewModel
    {
        /// <summary>
        /// Наименование поставщика коммунальных услуг.
        /// </summary>
        [Display(Name = "Наименование поставщика")]
        [Required(ErrorMessage = "Пожалуйста, введите наименование поставщика коммунальных услуг")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Наименование должно содержать от 5 до 100 символов")]
        public string Name { get; set; } = string.Empty;
    }
}
