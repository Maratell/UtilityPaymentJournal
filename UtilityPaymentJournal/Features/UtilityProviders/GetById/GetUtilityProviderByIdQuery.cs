using MediatR;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной записи поставщика услуг.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемого поставщика услуг.</param>
    public record GetUtilityProviderByIdQuery(long Id) : IRequest<GetUtilityProviderByIdResponse>;
}
