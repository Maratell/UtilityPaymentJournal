using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания показания счетчика электроэнергии.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateElectricityReadingMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность показания счетчика электроэнергии в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="ElectricityReading"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateElectricityReadingResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateElectricityReadingResponse ToResponse(this ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateElectricityReadingResponse(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        /// <summary>
        /// Создает новую доменную сущность показания счетчика электроэнергии на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="ElectricityReading"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static ElectricityReading ToEntity(this CreateElectricityReadingCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new ElectricityReading
            {
                ResidenceId = createCommand.ResidenceId,
                UtilityProviderId = createCommand.UtilityProviderId,
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
