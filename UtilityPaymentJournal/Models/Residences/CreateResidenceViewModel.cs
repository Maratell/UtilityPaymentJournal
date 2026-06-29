using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Models.Residences
{
    public class CreateResidenceViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(100, ErrorMessage = "Текст слишком длинный (максимум 100 символов)")]
        public string Address { get; set; } = string.Empty;
    }
}
