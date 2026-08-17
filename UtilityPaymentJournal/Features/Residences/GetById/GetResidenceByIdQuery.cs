using MediatR;

namespace UtilityPaymentJournal.Features.Residences.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной записи объекта недвижимости.
    /// Автоматически извлекает уникальный идентификатор из маршрута URL.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемого объекта недвижимости.</param>
    public record GetResidenceByIdQuery (long Id) : IRequest<GetResidenceByIdResponse>;
}
