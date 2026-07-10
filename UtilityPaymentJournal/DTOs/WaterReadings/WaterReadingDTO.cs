using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.WaterReadings
{
    public class WaterReadingDTO
    {
        public long Id { get; set; }

        public long? ResidenceId { get; set; }

        public long? UtilityProviderId { get; set; }

        public string? ResidenceAddress { get; set; } = string.Empty;

        public string? UtilityProviderName { get; set; } = string.Empty;

        public WaterType WaterType { get; set; }

        // Дата подачи показаний (может быть null)
        public DateTime? SubmissionDate { get; set; }

        // Дата оплаты (может быть null)
        public DateTime? PaymentDate { get; set; }

        public long CurrentValue { get; set; }

        public long PreviousValue { get; set; }

        public long ResultValue { get; set; }

        // Сумма платежа (используем decimal для финансовых данных)
        public decimal PaymentAmount { get; set; }
    }
}
