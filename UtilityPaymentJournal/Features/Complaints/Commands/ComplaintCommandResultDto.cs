using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// ДТО для возврата данных после выполнения команды над жалобой (ответ API на CUD операции).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор жалобы</param>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="CreatedAt">Дата и время создания записи в системе</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Дата и время фактического решения проблемы (опционально)</param>
    /// <param name="Status">Текущий статус рассмотрения жалобы</param>
    public record ComplaintCommandResultDto(
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
