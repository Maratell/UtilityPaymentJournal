using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Модель представления для создания новой жалобы (ввод данных из формы).
    /// </summary>
    public class CreateComplaintViewModel
    {
        /// <summary>
        /// Заголовок или краткая суть жалобы, обязательный для заполнения пользователем.
        /// </summary>
        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        [Display(Name = "Заголовок жалобы")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Подробный текст обращения с описанием сути проблемы.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, опишите суть проблемы")]
        [Display(Name = "Описание проблемы")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор выбранной пользователем коммунальной услуги из выпадающего списка.
        /// </summary>
        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Наименование услуги")]
        public long UtilityId { get; set; }
        /// <summary>
        /// Указываемая пользователем или системой дата официальной подачи жалобы.
        /// </summary>
        [Display(Name = "Дата подачи жалобы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата решения проблемы (заполняется при ручном закрытии или обновлении статуса).
        /// </summary>
        [Display(Name = "Дата решения проблемы")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Стартовый или обновляемый статус жалобы, проходящий валидацию перечисления.
        /// </summary>
        [Display(Name = "Статус жалобы")]
        [Required(ErrorMessage = "Укажите статус жалобы.")]
        [EnumDataType(typeof(ComplaintStatus), ErrorMessage = "Выбран некорректный статус жалобы.")]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;
    }
}
