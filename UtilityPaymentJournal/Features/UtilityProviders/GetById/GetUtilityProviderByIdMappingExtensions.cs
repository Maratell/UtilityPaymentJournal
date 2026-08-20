using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей поставщика услуг.
    /// </summary>
    public static class GetUtilityProviderByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность поставщика услуг в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="UtilityProvider"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetUtilityProviderByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равны null.</exception>
        public static GetUtilityProviderByIdResponse ToResponse(this UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetUtilityProviderByIdResponse(
                Id: entity.Id,
                Name: entity.Name
            );
        }
    }
}
