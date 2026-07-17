using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Models
{
    /// <summary>
    /// Модель представления для создания нового показания счетчика электроэнергии.
    /// </summary>
    public class CreateElectricityReadingViewModel
    {
        /// <summary>
        /// Идентификатор объекта недвижимости (дома или квартиры), для которого фиксируется новое показание.
        /// </summary>
        [Display(Name = "Жилой объект")]
        [Required(ErrorMessage = "Пожалуйста, выберите жилой объект")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID жилого объекта")]
        public long? ResidenceId { get; set; }
        /// <summary>
        /// Идентификатор поставщика услуг (энергосбытовой компании), осуществляющей снабжение электричеством.
        /// </summary>
        [Display(Name = "Поставщик услуг")]
        [Required(ErrorMessage = "Пожалуйста, выберите поставщика услуг")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID поставщика услуг")]
        public long? UtilityProviderId { get; set; }
        /// <summary>
        /// Дата и время официальной фиксации или передачи показаний. Может быть null.
        /// </summary>
        [Display(Name = "Дата подачи показаний")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Дата и время фактической оплаты выставленного счета. Заполняется, если оплачено сразу.
        /// </summary>
        [Display(Name = "Дата оплаты")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Свежее зафиксированное значение киловатт-часов на счетчике электроэнергии.
        /// </summary>
        [Display(Name = "Текущее показание")]
        [Required(ErrorMessage = "Пожалуйста, введите текущее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long CurrentValue { get; set; }
        /// <summary>
        /// Показание счетчика за прошлый расчетный период, используемое как стартовая точка.
        /// </summary>
        [Display(Name = "Показание за прошлый период")]
        [Required(ErrorMessage = "Пожалуйста, введите показание за прошлый период")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long PreviousValue { get; set; }
        /// <summary>
        /// Итоговый расход электроэнергии за текущий период (разница между текущим и прошлым).
        /// </summary>
        [Display(Name = "Расход (разница)")]
        [Required(ErrorMessage = "Пожалуйста, введите результирующее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long ResultValue { get; set; }
        /// <summary>
        /// Начисленная к оплате сумма за потребленную электроэнергию на основе тарифа.
        /// </summary>
        [Display(Name = "Сумма платежа")]
        [Required(ErrorMessage = "Пожалуйста, введите сумму платежа")]
        [Range(0.00, 999999.99, ErrorMessage = "Сумма должна быть положительным числом")]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }
    }
}
