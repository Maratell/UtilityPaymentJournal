namespace UtilityPaymentJournal.Features.Utilities.Models
{
    /// <summary>
    /// Развернутая модель представления коммунальной услуги для отображения на UI (ответ на GET).
    /// Отражает полные данные сущности справочника, включая информацию о датах изменения.
    /// </summary>
    public class UtilityDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи коммунальной услуги в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Наименование коммунальной услуги (например, "Водоснабжение", "Отопление")
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Класс иконки Bootstrap Icons для визуализации услуги в интерфейсе
        /// </summary>
        public string IconClass { get; set; } = string.Empty;
        /// <summary>
        /// Статус активности коммунальной услуги в системе
        /// </summary>
        public bool IsActive { get; set; }
    }
}
