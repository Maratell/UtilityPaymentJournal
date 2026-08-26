using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одной карточке жалобы.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор жалобы</param>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="UtilityName">Наименование связанной коммунальной услуги, подтянутое из БД</param>
    /// <param name="UtilityIcon">Ссылка на иконку коммунальной услуги, подтянутая из БД</param>
    /// <param name="CreatedAt">Дата и время автоматического создания записи в системе</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Дата и время фактического решения проблемы (опционально)</param>
    /// <param name="Status">Текущий статус рассмотрения жалобы в системе</param>
    public record GetComplaintByIdResponse(
        long Id,
        string Title,
        string Description,
        long UtilityId,
        string? UtilityName,         // Подтягивается из сущности Utility
        string? UtilityIcon,         // Подтягивается из сущности Utility
        DateTime CreatedAt,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    );
}
