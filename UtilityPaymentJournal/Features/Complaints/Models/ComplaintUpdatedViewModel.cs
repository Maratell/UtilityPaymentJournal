using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции обновления жалобы (ответ на PUT).
    /// Изолирована от модели создания для возможности независимого расширения метаданными апдейта.
    /// </summary>
    public class ComplaintUpdatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор измененной записи жалобы в БД.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Измененный заголовок или краткая суть жалобы.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Обновленное подробное текстовое описание проблемы.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор связанной коммунальной услуги.
        /// </summary>
        public long UtilityId { get; set; }
        /// <summary>
        /// Дата и время автоматического создания карточки с жалобой в системе (неизменяемое поле).
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Новая дата и время официальной подачи жалобы (если были изменены).
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Новая дата и время фактического устранения проблемы (заполняется при закрытии).
        /// </summary>
        public DateTime? IssueResolutionDate { get; set; }
        /// <summary>
        /// Обновленный статус жалобы в системе.
        /// </summary>
        public ComplaintStatus Status { get; set; }
    }
}
