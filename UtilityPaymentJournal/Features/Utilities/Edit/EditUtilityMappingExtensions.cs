using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Edit
{
    /// <summary>
    /// Методы расширения для локального маппинга фичи редактирования.
    /// </summary>
    public static class EditUtilityMappingExtensions
    {
        /// <summary>
        /// Переносит измененные данные из команды в существующую доменную сущность.
        /// </summary>
        /// <param name="command">Команда <see cref="EditUtilityCommand"/> на редактирование услуги.</param>
        /// <param name="entity">Доменная сущность <see cref="Utility"/>.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда или доменная сущность равны null.</exception>
        public static void UpdateEntity(this EditUtilityCommand command, Utility entity)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = command.Name;
            entity.IconClass = command.IconClass;
            entity.IsActive = command.IsActive;
        }

        /// <summary>
        /// Преобразует обновленную доменную сущность в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="Utility"/>.</param>
        /// <returns>Объект ответа API</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static EditUtilityResponse ToResponse(this Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new EditUtilityResponse(
                Id: entity.Id,
                Name: entity.Name,
                IconClass: entity.IconClass,
                IsActive: entity.IsActive
            );
        }
    }
}
