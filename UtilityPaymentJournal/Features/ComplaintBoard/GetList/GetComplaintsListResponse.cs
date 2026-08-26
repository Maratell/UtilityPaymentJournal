using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка карточек жалоб.
    /// </summary>
    public record GetComplaintsListResponse(IReadOnlyCollection<GetComplaintsListResponse.Item> Items)
    {
        public record Item(long Id, 
            string Title,
            string Description,
            long UtilityId,
            string? UtilityName,         
            string? UtilityIcon,       
            DateTime CreatedAt,
            DateTime? SubmissionDate,
            DateTime? IssueResolutionDate,
            ComplaintStatus Status
        );
    }
}
