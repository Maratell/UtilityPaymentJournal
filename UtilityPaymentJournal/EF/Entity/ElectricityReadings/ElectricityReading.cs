using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.EF.Entity.ElectricityReadings
{
    public class ElectricityReading : IAuditable, IUserOwned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Nullable-свойство для того, чтобы отменить каскадное удаление 
        /// (удаление жилого объекта не повлечет за собой удаление показания воды)
        /// </summary>
        public long? ResidenceId { get; set; }

        /// <summary>
        /// Связь один ко многим с жилыми объектами
        /// </summary>
        [ForeignKey(nameof(ResidenceId))]
        public Residence? Residence { get; set; }

        // Внешний ключ для связи с пользователем
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        /// <summary>
        /// Nullable-свойство для того, чтобы отменить каскадное удаление 
        /// (удаление поставщика услуг не повлечет за собой удаление показания воды)
        /// </summary>
        public long? UtilityProviderId { get; set; }

        /// <summary>
        /// Связь один ко многим с поставщиками услуг
        /// </summary>
        [ForeignKey(nameof(UtilityProviderId))]
        public UtilityProvider? UtilityProvider { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public long CurrentValue { get; set; }

        [Required]
        public long PreviousValue { get; set; }

        [Required]
        public long ResultValue { get; set; }

        /// <summary>
        /// Сумма платежа за расчетный объем воды
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
