using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка показаний счетчиков электроэнергии.
    /// </summary>
    public static class GetElectricityReadingsListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="ElectricityReading"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetElectricityReadingsListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetElectricityReadingsListResponse ToResponse(this IEnumerable<ElectricityReading> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetElectricityReadingsListResponse.Item[] items = entities
                .Select(e => new GetElectricityReadingsListResponse.Item(
                    Id: e.Id,
                    ResidenceId: e.ResidenceId,
                    UtilityProviderId: e.UtilityProviderId,
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
            return new GetElectricityReadingsListResponse(items);
        }
    }
}
