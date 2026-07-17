using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.Models
{
    /// <summary>
    /// Модель представления для редактирования существующего показания счетчика воды.
    /// Идентификатор изменяемой записи (Id) передается через маршрут URL.
    /// </summary>
    public class EditWaterReadingViewModel
    {
        /// <summary>
        /// Идентификатор объекта недвижимости (дома или квартиры).
        /// </summary>
        [Display(Name = "Жилой объект")]
        [Required(ErrorMessage = "Пожалуйста, выберите жилой объект")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID жилого объекта")]
        public long? ResidenceId { get; set; }
        /// <summary>
        /// Идентификатор поставщика коммунальной услуги.
        /// </summary>
        [Display(Name = "Поставщик услуг")]
        [Required(ErrorMessage = "Пожалуйста, выберите поставщика услуг")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID поставщика услуг")]
        public long? UtilityProviderId { get; set; }
        /// <summary>
        /// Тип воды (холодная или горячая).
        /// </summary>
        [Display(Name = "Тип ресурса")]
        [Required(ErrorMessage = "Укажите тип ресурса.")]
        [EnumDataType(typeof(WaterType), ErrorMessage = "Выбран некорректный тип ресурса.")]
        public WaterType WaterType { get; set; }
        /// <summary>
        /// Новая дата и время официальной подачи/фиксации показаний счетчика.
        /// </summary>
        [Display(Name = "Дата подачи показаний")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// Новая дата и время проведения оплаты по данному показанию.
        /// </summary>
        [Display(Name = "Дата оплаты")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Новое зафиксированное значение на счетчике воды.
        /// </summary>
        [Display(Name = "Текущее показание")]
        [Required(ErrorMessage = "Пожалуйста, введите текущее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long CurrentValue { get; set; }
        /// <summary>
        /// Новое сохраненное показание за прошлый период для перерасчета разницы.
        /// </summary>
        [Display(Name = "Показание за прошлый период")]
        [Required(ErrorMessage = "Пожалуйста, введите показание за прошлый период")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long PreviousValue { get; set; }
        /// <summary>
        /// Новое значение расхода воды за расчетный период (разница между текущим и прошлым).
        /// </summary>
        [Display(Name = "Расход (разница)")]
        [Required(ErrorMessage = "Пожалуйста, введите результирующее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long ResultValue { get; set; }
        /// <summary>
        /// Новая измененная сумма к оплате.
        /// </summary>
        [Display(Name = "Сумма платежа")]
        [Required(ErrorMessage = "Пожалуйста, введите сумму платежа")]
        [Range(0.00, 999999.99, ErrorMessage = "Сумма должна быть положительным числом")]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }
    }
}
