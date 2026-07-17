using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.Models
{
    /// <summary>
    /// Плоская модель представления для отображения данных о показании счетчика воды (ответ API).
    /// Не содержит вложенных объектов или навигационных свойств, передавая связанные данные в виде линейных строк.
    /// Используется как строгое описание выходного контракта.
    /// </summary>
    public class WaterReadingViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи показания в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор связанного жилого объекта
        /// </summary>
        public long? ResidenceId { get; set; }
        /// <summary>
        /// Идентификатор связанного поставщика услуг
        /// </summary>
        public long? UtilityProviderId { get; set; }
        /// <summary>
        /// Тип воды (холодная или горячая)
        /// </summary>
        public WaterType WaterType { get; set; }
        /// <summary>
        /// Полный текстовый адрес объекта (заполняется только при чтении)
        /// </summary>
        public string? ResidenceAddress { get; set; }
        /// <summary>
        /// Наименование поставщика услуг (заполняется только при чтении)
        /// </summary>
        public string? UtilityProviderName { get; set; }
        /// <summary>
        /// Дата подачи показаний
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата оплаты
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Текущее показание счетчика
        /// </summary>
        public long CurrentValue { get; set; }
        /// <summary>
        /// Показание за прошлый период
        /// </summary>
        public long PreviousValue { get; set; }
        /// <summary>
        /// Расход (разница показаний)
        /// </summary>
        public long ResultValue { get; set; }
        /// <summary>
        /// Итоговая сумма платежа
        /// </summary>
        public decimal PaymentAmount { get; set; }
    }
}
