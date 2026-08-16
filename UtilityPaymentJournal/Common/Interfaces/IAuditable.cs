namespace UtilityPaymentJournal.Common.Interfaces
{
    public interface IAuditable
    {
        public DateTime CreatedAt { get; set; }// = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
