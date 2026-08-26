using MediatR;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    /// <summary>
    /// Запрос на получение списка карточек жалоб.
    /// </summary>
    public record GetComplaintsListQuery : IRequest<GetComplaintsListResponse>;
}
