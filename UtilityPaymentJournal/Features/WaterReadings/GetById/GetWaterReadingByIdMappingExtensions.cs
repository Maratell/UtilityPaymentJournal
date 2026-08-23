using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей счетчика воды.
    /// </summary>
    public static class GetWaterReadingByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность счетчика воды в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="WaterReading"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetWaterReadingByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static GetWaterReadingByIdResponse ToResponse(this WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetWaterReadingByIdResponse(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                WaterType: entity.WaterType,
                ResidenceAddress: entity.Residence?.Address,
                UtilityProviderName: entity.UtilityProvider?.Name,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }
    }
}
