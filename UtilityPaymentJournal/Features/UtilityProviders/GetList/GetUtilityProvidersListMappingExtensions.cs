using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка поставщиков услуг.
    /// </summary>
    public static class GetUtilityProvidersListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="UtilityProvider"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetUtilityProvidersListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetUtilityProvidersListResponse ToResponse(this IEnumerable<UtilityProvider> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetUtilityProvidersListResponse.Item[] items = entities
                .Select(e => new GetUtilityProvidersListResponse.Item(
                    Id: e.Id,
                    Name: e.Name
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetUtilityProvidersListResponse(items);
        }
    }
}
