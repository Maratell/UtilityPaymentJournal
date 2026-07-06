namespace UtilityPaymentJournal.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToUtcKind(this DateTime? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.Value.ToUtcKind() 
                : null;
        }

        public static DateTime ToUtcKind(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue 
                ? dateTime.Value.ToLocalTime()
                : null;
        }

        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.Value.ToUniversalTime()
                : null;
        }
    }
}
