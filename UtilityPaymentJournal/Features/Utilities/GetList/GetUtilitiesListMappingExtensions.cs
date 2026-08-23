using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка поставщиков услуг.
    /// </summary>
    public static class GetUtilitiesListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="Utility"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetUtilitiesListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetUtilitiesListResponse ToResponse(this IEnumerable<Utility> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetUtilitiesListResponse.Item[] items = entities
                .Select(e => new GetUtilitiesListResponse.Item(
                    Id: e.Id,
                    Name: e.Name,
                    IconClass: e.IconClass,
                    IsActive: e.IsActive
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetUtilitiesListResponse(items);
        }
    }
}
