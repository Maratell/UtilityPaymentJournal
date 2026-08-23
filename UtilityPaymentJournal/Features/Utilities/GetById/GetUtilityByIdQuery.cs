using MediatR;

namespace UtilityPaymentJournal.Features.Utilities.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной записи услуги.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемой услуги.</param>
    public record GetUtilityByIdQuery(long Id) : IRequest<GetUtilityByIdResponse>;
}
