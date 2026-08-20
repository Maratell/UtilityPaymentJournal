using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    /// <summary>
    /// Методы расширения для локального маппинга фичи редактирования.
    /// </summary>
    public static class EditUtilityProviderMappingExtensions
    {
        /// <summary>
        /// Переносит измененные данные из команды в существующую доменную сущность.
        /// </summary>
        /// <param name="command">Команда <see cref="EditUtilityProviderCommand"/> на редактирование поставщика услуг.</param>
        /// <param name="entity">Доменная сущность <see cref="UtilityProvider"/>.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда или доменная сущность равны null.</exception>
        public static void UpdateEntity(this EditUtilityProviderCommand command, UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = command.Name;
        }

        /// <summary>
        /// Преобразует обновленную доменную сущность в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="UtilityProvider"/>.</param>
        /// <returns>Объект ответа API</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static EditUtilityProviderResponse ToResponse(this UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new EditUtilityProviderResponse(
                Id: entity.Id,
                Name: entity.Name
            );
        }
    }
}
