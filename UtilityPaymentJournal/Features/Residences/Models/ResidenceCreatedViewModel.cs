namespace UtilityPaymentJournal.Features.Residences.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания объекта недвижимости (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая только факт успешной генерации записи.
    /// </summary>
    public class ResidenceCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи объекта недвижимости в БД.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Полный адрес созданного объекта недвижимости.
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
