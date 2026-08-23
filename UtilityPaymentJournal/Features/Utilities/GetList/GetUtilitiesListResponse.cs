
namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка услуг.
    /// </summary>
    public record GetUtilitiesListResponse(IReadOnlyCollection<GetUtilitiesListResponse.Item> Items)
    {
        public record Item(
            long Id, 
            string Name,
            string IconClass,
            bool IsActive
        );
    }
}
