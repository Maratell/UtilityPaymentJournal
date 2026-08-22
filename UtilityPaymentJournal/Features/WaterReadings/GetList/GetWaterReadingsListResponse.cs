using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка недвижимости.
    /// </summary>
    /// <param name="Items">Коллекция элементов списка, использующая вложенный тип <see cref="Item"/>.</param>
    public record GetWaterReadingsListResponse(IReadOnlyCollection<GetWaterReadingsListResponse.Item> Items)
    {
        public record Item(
            long Id,
            long? ResidenceId,
            long? UtilityProviderId,
            WaterType WaterType,
            string? ResidenceAddress,     
            string? UtilityProviderName,   
            DateTime? SubmissionDate,
            DateTime? PaymentDate,
            long CurrentValue,
            long PreviousValue,
            long ResultValue,
            decimal PaymentAmount
        );
    }
}
