using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Развернутая модель представления жалобы для отображения на UI (ответ на GET).
    /// Гарантированно содержит текстовые данные связанных сущностей, так как запрашивается через Include.
    /// </summary>
    public class ComplaintDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи жалобы в БД.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Заголовок или краткая суть жалобы, описывающая проблему.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Подробное текстовое описание возникшей проблемы или инцидента.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор связанной коммунальной услуги в системе.
        /// </summary>
        public long UtilityId { get; set; }
        /// <summary>
        /// Наименование связанной коммунальной услуги, подтянутое из БД.
        /// </summary>
        public string? UtilityName { get; set; }
        /// <summary>
        /// Ссылка на иконку коммунальной услуги для отображения в UI, подтянутая из БД.
        /// </summary>
        public string? UtilityIcon { get; set; }
        /// <summary>
        /// Дата и время автоматического создания карточки с жалобой в системе.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата и время официальной подачи или регистрации жалобы.
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время фактического устранения проблемы и закрытия жалобы.
        /// </summary>
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Текущий статус рассмотрения жалобы (например, Новая, В работе, Решена).
        /// </summary>
        public ComplaintStatus Status { get; set; }
    }
}
