using UtilityPaymentJournal.Enumerations;

namespace UtilityPaymentJournal.DTO.ElectricityReadings
{
    public class EditElectricityReadingDTO
    {
        public long Id { get; set; }
        public long? ResidenceId { get; set; }
        public long? UtilityProviderId { get; set; }

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
