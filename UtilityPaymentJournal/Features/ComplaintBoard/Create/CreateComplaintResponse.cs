using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданной карточки жалобы с присвоенным идентификатором.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор карточки далобы, сгенерированный базой данных.</param>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="CreatedAt">Дата и время создания записи в системе</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Дата и время фактического решения проблемы (опционально)</param>
    /// <param name="Status">Текущий статус рассмотрения жалобы</param>
    public record CreateComplaintResponse
    (
        long Id,
        string Title,
        string Description,
        long? UtilityId,
        DateTime CreatedAt,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    );
}
