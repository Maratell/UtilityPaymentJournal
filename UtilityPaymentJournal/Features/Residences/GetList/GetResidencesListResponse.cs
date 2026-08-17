namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Единый объект ответа API для фичи получения списка недвижимости.
    /// </summary>
    /// <param name="Items">Коллекция элементов списка, использующая вложенный тип <see cref="Item"/>.</param>
    /// <remarks>
    /// На стороне клиента (фронтенд) этот рекорд сериализуется в чистый JSON-объект с массивом:
    /// <code>
    /// {
    ///   "items": [
    ///     { "id": 1, "address": "..." }
    ///   ]
    /// }
    /// </code>
    /// </remarks>
    public record GetResidencesListResponse(IReadOnlyCollection<GetResidencesListResponse.Item> Items)
    {
        /// <summary>
        /// Вложенный (Nested) тип, описывающий структуру ОДНОГО элемента в списке.
        /// </summary>
        /// <param name="Id">Уникальный идентификатор объекта недвижимости.</param>
        /// <param name="Address">Полный текстовый адрес объекта недвижимости.</param>
        /// <remarks>
        /// Это НЕ поле с данными, а только ТИП данных (чертеж), а вот Items - это поле, 
        /// которое уже использует описанный тип Item
        /// Добавление этого типа решает две важные задачи:
        /// 1. Избавляет от создания отдельного элемента списка.
        /// 2. Защищает от конфликтов имен — в других фичах списков можно также создавать свои локальные рекорды "Item".
        /// </remarks>
        public record Item(long Id, string Address);
    }
}
