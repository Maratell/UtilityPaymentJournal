using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.ComplaintBoard
{
    /// <summary>
    /// ДТО для редактирования существующей жалобы.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор жалобы в бд</param>
    /// <param name="Title">Новый заголовок или краткая суть жалобы</param>
    /// <param name="Description">Новое подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор новой связанной коммунальной услуги</param>
    /// <param name="CreatedAt">Дата и время создания записи (передается для сохранения истории)</param>
    /// <param name="SubmissionDate">Новая дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Новая дата и время решения проблемы (опционально)</param>
    /// <param name="Status">Новый статус рассмотрения жалобы</param>
    public record EditComplaintDto(
        long Id,
        string Title,
        string Description,
        long UtilityId,
        DateTime CreatedAt,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    );
}
