using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.Edit
{
    /// <summary>
    /// Методы расширения для локального маппинга фичи редактирования.
    /// </summary>
    public static class EditWaterReadingMappingExtensions
    {
        /// <summary>
        /// Переносит измененные данные из команды в существующую доменную сущность.
        /// </summary>
        /// <param name="command">Команда <see cref="EditWaterReadingCommand"/> на редактирование.</param>
        /// <param name="entity">Доменная сущность <see cref="WaterReading"/>.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда или доменная сущность равны null.</exception>
        public static void UpdateEntity(this EditWaterReadingCommand command, WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(entity);

            entity.ResidenceId = command.ResidenceId;
            entity.UtilityProviderId = command.UtilityProviderId;
            entity.WaterType = command.WaterType;
            entity.SubmissionDate = command.SubmissionDate; //.ToUtcKind();
            entity.PaymentDate = command.PaymentDate; //.ToUtcKind();
            entity.CurrentValue = command.CurrentValue;
            entity.PreviousValue = command.PreviousValue;
            entity.ResultValue = command.ResultValue;
            entity.PaymentAmount = command.PaymentAmount;
        }

        /// <summary>
        /// Преобразует обновленную доменную сущность в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность.</param>
        /// <returns>Объект ответа API <see cref="EditWaterReadingResponse"/></returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static EditWaterReadingResponse ToResponse(this WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new EditWaterReadingResponse(
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
    }
}
