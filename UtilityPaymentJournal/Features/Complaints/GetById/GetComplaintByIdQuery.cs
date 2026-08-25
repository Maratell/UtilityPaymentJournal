using MediatR;

namespace UtilityPaymentJournal.Features.Complaints.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной карточки жалобы.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемой карточки жалобы.</param>
    public record GetComplaintByIdQuery(long Id) : IRequest<GetComplaintByIdResponse>;
}
