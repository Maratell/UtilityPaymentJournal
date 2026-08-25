using MediatR;

namespace UtilityPaymentJournal.Features.Complaints.GetList
{
    /// <summary>
    /// Запрос на получение списка карточек жалоб.
    /// </summary>
    public record GetComplaintsListQuery : IRequest<GetComplaintsListResponse>;
}
