using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Плоская модель представления для отображения данных о карточке-жалобе (ответ API).
    /// Не содержит вложенных объектов или навигационных свойств, передавая связанные данные в виде линейных строк.
    /// Используется как строгое описание выходного контракта.
    /// </summary>
    public class ComplaintViewModel
    {
        /// <summary>
        /// Уникальный идентификатор жалобы в системе.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Заголовок или краткая суть жалобы, отображаемая в интерфейсе.
        /// </summary>
        [Display(Name = "Заголовок жалобы")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Подробное текстовое описание проблемы.
        /// </summary>
        [Display(Name = "Описание проблемы")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Уникальный идентификатор связанной коммунальной услуги.
        /// </summary>
        public long UtilityId { get; set; }
        /// <summary>
        /// Наименование связанной коммунальной услуги, подтянутое из базы данных.
        /// </summary>
        [Display(Name = "Поставщик")]
        public string? UtilityName { get; set; }
        /// <summary>
        /// Относительный путь или URL иконки коммунальной услуги, подтянутый из базы данных.
        /// </summary>
        public string? UtilityIcon { get; set; }
        /// <summary>
        /// Дата и время автоматического создания карточки с жалобой в системе.
        /// </summary>
        [Display(Name = "Дата создания карточки с жалобой")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата и время официальной подачи жалобы (может быть не заполнено).
        /// </summary>
        [Display(Name = "Дата подачи жалобы")]
        [DataType(DataType.Date)]
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время фактического устранения проблемы и закрытия жалобы.
        /// </summary>
        [Display(Name = "Дата решения проблемы")]
        [DataType(DataType.Date)]
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Текущий статус рассмотрения и обработки жалобы.
        /// </summary>
        [Display(Name = "Статус жалобы")]
        public ComplaintStatus Status { get; set; }
    }
}
