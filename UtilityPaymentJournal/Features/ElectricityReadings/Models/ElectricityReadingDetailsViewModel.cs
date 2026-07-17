namespace UtilityPaymentJournal.Features.ElectricityReadings.Models
{
    /// <summary>
    /// Развернутая модель представления показания счетчика электроэнергии для отображения на UI (ответ на GET).
    /// Гарантированно содержит текстовые данные связанных сущностей, так как запрашивается через Include.
    /// </summary>
    public class ElectricityReadingDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи показания в БД
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
        /// Полный текстовый адрес жилого помещения, подтянутый из БД
        /// </summary>
        public string? ResidenceAddress { get; set; }
        /// <summary>
        /// Наименование компании поставщика услуги, подтянутое из БД
        /// </summary>
        public string? UtilityProviderName { get; set; }
        /// <summary>
        /// Дата и время официальной подачи/фиксации показаний счетчика
        /// </summary>
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время проведения оплаты по данному показанию (опционально)
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Текущее зафиксированное значение на счетчике электроэнергии
        /// </summary>
        public long CurrentValue { get; set; }
        /// <summary>
        /// Предыдущее зафиксированное значение на счетчике для расчета разницы
        /// </summary>
        public long PreviousValue { get; set; }
        /// <summary>
        /// Итоговый расход электроэнергии за расчетный период
        /// </summary>
        public long ResultValue { get; set; }
        /// <summary>
        /// Сумма к оплате, рассчитанная на основе итогового расхода и тарифа
        /// </summary>
        public decimal PaymentAmount { get; set; }
    }
}
