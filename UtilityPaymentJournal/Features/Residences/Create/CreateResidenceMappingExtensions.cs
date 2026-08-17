using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания объекта недвижимости.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateResidenceMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность недвижимости в объект ответа API.
        /// </summary>
        /// Доменная сущность <see cref="Residence"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateResidenceResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateResidenceResponse ToResponse(this Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateResidenceResponse(
                Id: entity.Id,
                Address: entity.Address
            );
        }

        /// <summary>
        /// Создает новую доменную сущность недвижимости на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="Residence"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static Residence ToEntity(this CreateResidenceCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new Residence
            {
                Address = createCommand.Address
            };
        }
    }
}
