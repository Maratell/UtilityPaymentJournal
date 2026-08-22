using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка объектов недвижимости.
    /// </summary>
    public static class GetWaterReadingsListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="WaterReading"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetWaterReadingsListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetWaterReadingsListResponse ToResponse(this IEnumerable<WaterReading> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetWaterReadingsListResponse.Item[] items = entities
                .Select(e => new GetWaterReadingsListResponse.Item(
                    Id: e.Id,
                    ResidenceId: e.ResidenceId,
                    UtilityProviderId: e.UtilityProviderId,
                    WaterType: e.WaterType,
                    ResidenceAddress: e.Residence?.Address,
                    UtilityProviderName: e.UtilityProvider?.Name,
                    SubmissionDate: e.SubmissionDate,
                    PaymentDate: e.PaymentDate,
                    CurrentValue: e.CurrentValue,
                    PreviousValue: e.PreviousValue,
                    ResultValue: e.ResultValue,
                    PaymentAmount: e.PaymentAmount
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetWaterReadingsListResponse(items);
        }
    }
}
