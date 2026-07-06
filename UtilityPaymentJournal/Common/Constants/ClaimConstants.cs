namespace UtilityPaymentJournal.Common.Constants
{
    /// <summary>
    /// Константы типов утверждений (Claims) для всей системы.
    /// Предотвращают появление ошибок из-за опечаток в строках (Magic Strings).
    /// </summary>
    public static class ClaimConstants
    {
        /// <summary>
        /// Клейм для хранения имени пользователя в авторизационной куке.
        /// </summary>
        public const string FirstName = "app_first_name";

        /// <summary>
        /// Клейм для хранения фамилии пользователя в авторизационной куке.
        /// </summary>
        public const string LastName = "app_last_name";
    }
}
