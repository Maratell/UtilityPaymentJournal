using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Models.Utilities
{
    public class CreateUtilityViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(100, ErrorMessage = "Текст слишком длинный (максимум 100 символов)")]
        public string Name { get; set; } = string.Empty;
    }
}
