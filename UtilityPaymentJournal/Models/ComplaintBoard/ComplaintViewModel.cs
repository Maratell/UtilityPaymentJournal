using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Enumerations;

namespace UtilityPaymentJournal.Models.ComplaintBoard
{
    public class ComplaintViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        [Display(Name = "Заголовок жалобы")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пожалуйста, опишите суть проблемы")]
        [Display(Name = "Описание проблемы")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Наименование услуги")]
        public long UtilityId { get; set; }

        [Display(Name = "Поставщик")]
        public string? UtilityName { get; set; }

        public string? UtilityIcon { get; set; }

        [Display(Name = "Дата создания карточки с жалобой")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Дата подачи жалобы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }

        [Display(Name = "Дата решения проблемы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? IssueResolutionDate { get; set; }

        [Display(Name = "Статус жалобы")]
        [Required(ErrorMessage = "Укажите статус жалобы.")]
        [EnumDataType(typeof(ComplaintStatus), ErrorMessage = "Выбран некорректный статус жалобы.")]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;
    }
}
