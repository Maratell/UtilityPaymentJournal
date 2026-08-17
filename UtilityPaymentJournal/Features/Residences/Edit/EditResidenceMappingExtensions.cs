using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Методы расширения для локального маппинга фичи редактирования.
    /// </summary>
    public static class EditResidenceMappingExtensions
    {
        /// <summary>
        /// Переносит измененные данные из команды в существующую доменную сущность.
        /// </summary>
        public static void UpdateEntity(this EditResidenceCommand command, Residence entity)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Address = command.Address;
        }

        /// <summary>
        /// Преобразует обновленную доменную сущность в объект ответа API.
        /// </summary>
        public static EditResidenceResponse ToResponse(this Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new EditResidenceResponse(
                Id: entity.Id,
                Address: entity.Address
            );
        }
    }
}
