using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.ComplaintBoard
{
    /// <summary>
    /// ДТО для создания новой жалобы.
    /// </summary>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="CreatedAt">Дата и время создания записи (по умолчанию текущее время UTC)</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Планируемая дата и время решения проблемы (опционально)</param>
    /// <param name="Status">Начальный статус жалобы (по умолчанию Новая)</param>
    public record CreateComplaintDto(
        string Title,
        string Description,
        long UtilityId,
        DateTime CreatedAt, 
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status 
    );
}
