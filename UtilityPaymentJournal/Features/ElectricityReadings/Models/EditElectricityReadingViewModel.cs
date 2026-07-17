using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Models
{
    /// <summary>
    /// Модель представления для редактирования существующего показания счетчика электроэнергии.
    /// Идентификатор изменяемой записи (Id) передается через маршрут URL.
    /// </summary>
    public class EditElectricityReadingViewModel
    {
        [Display(Name = "Жилой объект")]
        [Required(ErrorMessage = "Пожалуйста, выберите жилой объект")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID жилого объекта")]
        public long? ResidenceId { get; set; }

        [Display(Name = "Поставщик услуг")]
        [Required(ErrorMessage = "Пожалуйста, выберите поставщика услуг")]
        [Range(1, long.MaxValue, ErrorMessage = "Некорректный ID поставщика услуг")]
        public long? UtilityProviderId { get; set; }

        [Display(Name = "Дата подачи показаний")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? SubmissionDate { get; set; }

        [Display(Name = "Дата оплаты")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Текущее показание")]
        [Required(ErrorMessage = "Пожалуйста, введите текущее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long CurrentValue { get; set; }

        [Display(Name = "Показание за прошлый период")]
        [Required(ErrorMessage = "Пожалуйста, введите показание за прошлый период")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long PreviousValue { get; set; }

        [Display(Name = "Расход (разница)")]
        [Required(ErrorMessage = "Пожалуйста, введите результирующее показание")]
        [Range(0, long.MaxValue, ErrorMessage = "Показание должно быть целым положительным числом")]
        public long ResultValue { get; set; }

        [Display(Name = "Сумма платежа")]
        [Required(ErrorMessage = "Пожалуйста, введите сумму платежа")]
        [Range(0.00, 999999.99, ErrorMessage = "Сумма должна быть положительным числом")]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }
    }
}
