using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    /// <summary>
    /// Запрос на создание карточки жалобы
    /// </summary>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (может быть null)</param>
    /// <param name="IssueResolutionDate">Дата и время фактического решения проблемы (может быть null)</param>
    /// <param name="Status">Текущий статус рассмотрения жалобы</param>
    public record CreateComplaintRequest(
        string Title,
        string Description,
        long? UtilityId,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    )
    {
        public CreateComplaintCommand ToCommand() =>
            new CreateComplaintCommand(
                Title,
                Description, 
                UtilityId,
                SubmissionDate,
                IssueResolutionDate,
                Status 
            );
    }
}
