using MediatR;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetList
{
    /// <summary>
    /// Запрос на получение списка поставщиков услуг.
    /// </summary>
    public record GetUtilityProvidersListQuery : IRequest<GetUtilityProvidersListResponse>;
}
