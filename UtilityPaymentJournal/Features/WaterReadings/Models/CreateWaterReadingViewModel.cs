using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.Models
{
    /// <summary>
    /// Модель представления для создания нового показания счетчика воды.
    /// </summary>
    public class CreateWaterReadingViewModel
    {
        /// <summary>
        /// Идентификатор объекта недвижимости (дома или квартиры), для которого фиксируется новое показание.
        /// </summary>
        [Display(Name = "Жилой объект")]
        [Required(ErrorMessage = "Пожалуйста, выберите жилой объект")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID жилого объекта")]
        public long? ResidenceId { get; set; }
        /// <summary>
        /// Идентификатор поставщика услуг, осуществляющего снабжение водой по данному адресу.
        /// </summary>
        [Display(Name = "Поставщик услуг")]
        [Required(ErrorMessage = "Пожалуйста, выберите поставщика услуг")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID поставщика услуг")]
        public long? UtilityProviderId { get; set; }
        /// <summary>
        /// Тип воды (холодная или горячая).
        /// </summary>
        [Required(ErrorMessage = "Укажите тип ресурса.")]
        [EnumDataType(typeof(WaterType), ErrorMessage = "Выбран некорректный тип ресурса.")]
        [Display(Name = "Тип ресурса")]
        public WaterType WaterType { get; set; }
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
        /// Свежее зафиксированное значение кубических метров на счетчике воды.
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
        /// Итоговый расход воды за текущий период (разница между текущим и прошлым).
        /// </summary>
        [Display(Name = "Расход (разница)")]
        [Required(ErrorMessage = "Пожалуйста, введите результирующее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long ResultValue { get; set; }
        /// <summary>
        /// Начисленная к оплате сумма за потребленный объем воды на основе тарифа.
        /// </summary>
        [Display(Name = "Сумма платежа")]
        [Required(ErrorMessage = "Пожалуйста, введите сумму платежа")]
        [Range(0.00, 999999.99, ErrorMessage = "Сумма должна быть положительным числом")]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }
    }
}
