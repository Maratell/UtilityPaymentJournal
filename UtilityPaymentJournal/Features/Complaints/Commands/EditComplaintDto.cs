using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// ДТО для редактирования существующей жалобы.
    /// </summary>
    /// <param name="Title">Новый заголовок или краткая суть жалобы</param>
    /// <param name="Description">Новое подробное описание проблемы</param>
    /// <param name="UtilityId">Новый идентификатор связанной коммунальной услуги</param>
    /// <param name="SubmissionDate">Новая дата и время официальной подачи жалобы (может быть null)</param>
    /// <param name="IssueResolutionDate">Новая дата и время фактического решения проблемы (может быть null)</param>
    /// <param name="Status">Новый статус рассмотрения жалобы</param>
    public record EditComplaintDto(
        string Title,
        string Description,
        long UtilityId,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    );
}
