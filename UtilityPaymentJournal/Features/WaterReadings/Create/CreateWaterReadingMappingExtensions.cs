using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания показания счетчика воды.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateWaterReadingMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность показания счетчика воды в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="WaterReading"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateWaterReadingResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateWaterReadingResponse ToResponse(this WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateWaterReadingResponse(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                WaterType: entity.WaterType,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        /// <summary>
        /// Создает новую доменную сущность показания счетчика воды на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="Residence"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static WaterReading ToEntity(this CreateWaterReadingCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new WaterReading
            {
                ResidenceId = createCommand.ResidenceId,
                UtilityProviderId = createCommand.UtilityProviderId,
                WaterType = createCommand.WaterType,
                SubmissionDate = createCommand.SubmissionDate, //.ToUtcKind(),
                PaymentDate = createCommand.PaymentDate, //.ToUtcKind(),
                CurrentValue = createCommand.CurrentValue,
                PreviousValue = createCommand.PreviousValue,
                ResultValue = createCommand.ResultValue,
                PaymentAmount = createCommand.PaymentAmount
            };
        }
    }
}
