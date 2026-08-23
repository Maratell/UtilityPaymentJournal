using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей поставщика услуг.
    /// </summary>
    public static class GetUtilityByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность услуги в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="Utility"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetUtilityByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равны null.</exception>
        public static GetUtilityByIdResponse ToResponse(this Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetUtilityByIdResponse(
                Id: entity.Id,
                Name: entity.Name,
                IconClass: entity.IconClass,
                IsActive: entity.IsActive
            );
        }
    }
}
