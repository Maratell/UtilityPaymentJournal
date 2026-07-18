using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания жалобы (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая только факт успешной генерации записи.
    /// </summary>
    public class ComplaintCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи жалобы в БД.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Заголовок или краткая суть созданной жалобы.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Подробное текстовое описание проблемы.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор связанной коммунальной услуги.
        /// </summary>
        public long UtilityId { get; set; }
        /// <summary>
        /// Дата и время автоматического создания карточки с жалобой в системе.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата и время официальной подачи жалобы (если были зафиксированы).
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время фактического устранения проблемы (если была закрыта сразу).
        /// </summary>
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Стартовый статус созданной жалобы в системе.
        /// </summary>
        public ComplaintStatus Status { get; set; }
    }
}
