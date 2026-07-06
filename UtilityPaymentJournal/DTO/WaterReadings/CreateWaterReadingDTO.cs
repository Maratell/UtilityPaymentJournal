using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTO.WaterReadings
{
    public class CreateWaterReadingDTO
    {
        public long ResidenceId { get; set; }
        public long UtilityProviderId { get; set; }
        public WaterType WaterType { get; set; }

        // Дата подачи показаний (обязательная)
        public DateTime? SubmissionDate { get; set; }

        // Дата оплаты (может быть null, если еще не оплачено)
        public DateTime? PaymentDate { get; set; }

        public long CurrentValue { get; set; }

        public long PreviousValue { get; set; }

        public long ResultValue { get; set; }

        // Сумма платежа (используем decimal для финансовых данных)
        public decimal PaymentAmount { get; set; }
    }
}
