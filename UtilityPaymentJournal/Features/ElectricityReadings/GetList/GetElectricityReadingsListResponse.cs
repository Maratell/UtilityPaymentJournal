
namespace UtilityPaymentJournal.Features.ElectricityReadings.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка показаний счетчиков электроэнергии.
    /// </summary>
    /// <param name="Items">Коллекция элементов списка, использующая вложенный тип <see cref="Item"/>.</param>
    public record GetElectricityReadingsListResponse(IReadOnlyCollection<GetElectricityReadingsListResponse.Item> Items)
    {
        public record Item(
            long Id,
            long? ResidenceId,
            long? UtilityProviderId,
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
