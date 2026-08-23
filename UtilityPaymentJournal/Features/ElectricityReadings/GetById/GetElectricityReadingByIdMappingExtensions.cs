using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей счетчика электроэнергии.
    /// </summary>
    public static class GetElectricityReadingByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность счетчика электроэнергии в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="ElectricityReading"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetElectricityReadingByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static GetElectricityReadingByIdResponse ToResponse(this ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetElectricityReadingByIdResponse(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
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
