namespace UtilityPaymentJournal.Features.Utilities.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания коммунальной услуги (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая только факт успешной генерации записи.
    /// </summary>
    public class UtilityCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи коммунальной услуги в БД.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Наименование созданной коммунальной услуги.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Класс иконки Bootstrap Icons, назначенный созданной услуге.
        /// </summary>
        public string IconClass { get; set; } = string.Empty;
        /// <summary>
        /// Статус активности созданной коммунальной услуги в системе.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
