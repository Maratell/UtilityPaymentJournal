namespace UtilityPaymentJournal.Features.Utilities.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции обновления коммунальной услуги (ответ на PUT).
    /// Изолирована от модели создания для возможности независимого расширения метаданными апдейта.
    /// </summary>
    public class UtilityUpdatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор измененной записи коммунальной услуги в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Наименование измененной коммунальной услуги
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Класс иконки Bootstrap Icons, назначенный услуге
        /// </summary>
        public string IconClass { get; set; } = string.Empty;
        /// <summary>
        /// Статус активности измененной коммунальной услуги в системе
        /// </summary>
        public bool IsActive { get; set; }
    }
}
