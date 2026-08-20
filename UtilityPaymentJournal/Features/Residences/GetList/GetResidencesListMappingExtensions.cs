using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка объектов недвижимости.
    /// </summary>
    public static class GetResidencesListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="Residence"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetResidencesListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetResidencesListResponse ToResponse(this IEnumerable<Residence> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetResidencesListResponse.Item[] items = entities
                .Select(e => new GetResidencesListResponse.Item(
                    Id: e.Id,
                    Address: e.Address
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetResidencesListResponse(items);
        }
    }
}
