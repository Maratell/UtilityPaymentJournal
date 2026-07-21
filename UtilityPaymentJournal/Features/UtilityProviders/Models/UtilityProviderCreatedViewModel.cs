namespace UtilityPaymentJournal.Features.UtilityProviders.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания поставщика коммунальных услуг (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая только факт успешной генерации записи.
    /// </summary>
    public class UtilityProviderCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи поставщика в БД.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование созданного поставщика услуг.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
