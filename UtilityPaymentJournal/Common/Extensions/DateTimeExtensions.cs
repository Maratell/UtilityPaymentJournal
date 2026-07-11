namespace UtilityPaymentJournal.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToUtcKind(this DateTime? dateTime)
        {
            return dateTime?.ToUtcKind();
        }

        public static DateTime ToUtcKind(this DateTime dateTime)
        {
            //return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            // Если .NET уже знает, что эта дата в UTC, то ничего делать не нужно — возвращаем её как есть
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            // Если тип даты «Не указан» (после десериализации из JSON), мы просто вешаем ярлык "это UTC".
            // Часы и минуты при этом не двигаются в зависимости от часового пояса на котором работает сервер,
            // что спасает от багов с отображением даты.
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime?.ToLocalTime();
        }
    }
}
