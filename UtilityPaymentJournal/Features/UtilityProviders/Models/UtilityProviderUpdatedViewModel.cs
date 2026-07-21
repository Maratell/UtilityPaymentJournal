namespace UtilityPaymentJournal.Features.UtilityProviders.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции обновления поставщика коммунальных услуг (ответ на PUT).
    /// Изолирована от модели создания для возможности независимого расширения метаданными апдейта.
    /// </summary>
    public class UtilityProviderUpdatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор измененной записи поставщика в БД.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Новое наименование поставщика услуг.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
