
namespace UtilityPaymentJournal.Features.UtilityProviders.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка поставщиков услуг.
    /// </summary>
    public record GetUtilityProvidersListResponse(IReadOnlyCollection<GetUtilityProvidersListResponse.Item> Items)
    {
        public record Item(long Id, string Name);
    }
}
