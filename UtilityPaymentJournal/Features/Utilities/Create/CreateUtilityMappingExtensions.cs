using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания услуги.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateUtilityMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность услуги в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="Utility"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateUtilityResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateUtilityResponse ToResponse(this Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateUtilityResponse(
                Id: entity.Id,
                Name: entity.Name,
                IconClass: entity.IconClass,
                IsActive: entity.IsActive
            );
        }

        /// <summary>
        /// Создает новую доменную сущность услуги на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="Utility"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static Utility ToEntity(this CreateUtilityCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new Utility
            {
                Name = createCommand.Name,
                IconClass = createCommand.IconClass,
                IsActive = createCommand.IsActive
            };
        }
    }
}
