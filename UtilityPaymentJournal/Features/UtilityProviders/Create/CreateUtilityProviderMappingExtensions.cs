using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания поставщика услуг.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateUtilityProviderMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность поставщика услуг в объект ответа API.
        /// </summary>
        /// Доменная сущность <see cref="UtilityProvider"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateUtilityProviderResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateUtilityProviderResponse ToResponse(this UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateUtilityProviderResponse(
                Id: entity.Id,
                Name: entity.Name
            );
        }

        /// <summary>
        /// Создает новую доменную сущность поставщика услуг на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="UtilityProvider"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static UtilityProvider ToEntity(this CreateUtilityProviderCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new UtilityProvider
            {
                Name = createCommand.Name
            };
        }
    }
}
