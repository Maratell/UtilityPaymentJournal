using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания показания счетчика воды (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая только факт успешной генерации записи.
    /// </summary>
    public class WaterReadingCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи показания в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор связанного жилого помещения (дома/квартиры)
        /// </summary>
        public long? ResidenceId { get; set; }
        /// <summary>
        /// Идентификатор связанного поставщика коммунальной услуги
        /// </summary>
        public long? UtilityProviderId { get; set; }
        /// <summary>
        /// Тип воды (холодная или горячая).
        /// </summary>
        public WaterType WaterType { get; set; }
        /// <summary>
        /// Дата и время официальной подачи/фиксации показаний счетчика
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время проведения оплаты по данному показанию (опционально)
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Текущее зафиксированное значение на счетчике воды
        /// </summary>
        public long CurrentValue { get; set; }
        /// <summary>
        /// Предыдущее зафиксированное значение на счетчике для расчета разницы
        /// </summary>
        public long PreviousValue { get; set; }
        /// <summary>
        /// Итоговый расход воды за расчетный период
        /// </summary>
        public long ResultValue { get; set; }
        /// <summary>
        /// Сумма к оплате, рассчитанная на основе итогового расхода и тарифа
        /// </summary>
        public decimal PaymentAmount { get; set; }
    }
}
