namespace UtilityPaymentJournal.Features.UtilityProviders.Models
{
    /// <summary>
    /// Развернутая модель представления поставщика коммунальных услуг для отображения на UI (ответ на GET).
    /// Содержит полную текстовую и идентификационную информацию о поставщике.
    /// </summary>
    public class UtilityProviderDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи поставщика в БД.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Полное наименование поставщика услуг.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
