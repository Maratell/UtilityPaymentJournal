namespace UtilityPaymentJournal.Common.Constants
{
    /// <summary>
    /// Глобальные константы безопасности для защиты от CSRF/Antiforgery-атак и аутентификации.
    /// </summary>
    public static class AntiforgerySecurityConstants
    {
        /// <summary>
        /// Имя HTTP-заголовка для передачи токена защиты от межсайтовой подделки запросов (CSRF).
        /// Используется на HTML-страницах фронтенда (JS/jQuery) и в спецификации Swagger.
        /// </summary>
        public const string AntiforgeryHeaderName = "RequestVerificationToken";
    }
}
