using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Модель представления для редактирования существующей жалобы.
    /// </summary>
    public class EditComplaintViewModel
    {
        /// <summary>
        /// Уникальный идентификатор редактируемой жалобы в системе.
        /// </summary>
        [Required]
        public long Id { get; set; }
        /// <summary>
        /// Измененный заголовок или краткая суть жалобы.
        /// </summary>
        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        [Display(Name = "Заголовок жалобы")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Обновленное подробное текстовое описание проблемы.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, опишите суть проблемы")]
        [Display(Name = "Описание проблемы")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор измененной коммунальной услуги из выпадающего списка.
        /// </summary>
        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Наименование услуги")]
        public long UtilityId { get; set; }
        /// <summary>
        /// Новая дата официальной подачи жалобы (может быть null).
        /// </summary>
        [Display(Name = "Дата подачи жалобы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Новая дата и время фактического устранения проблемы (заполняется при закрытии).
        /// </summary>
        [Display(Name = "Дата решения проблемы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Измененный текущий статус рассмотрения жалобы.
        /// </summary>
        [Display(Name = "Статус жалобы")]
        [Required(ErrorMessage = "Укажите статус жалобы.")]
        [EnumDataType(typeof(ComplaintStatus), ErrorMessage = "Выбран некорректный статус жалобы.")]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;
    }
}
