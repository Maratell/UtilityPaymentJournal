namespace UtilityPaymentJournal.Infrastructure.EF.Constants
{
    /// <summary>
    /// Глобальные конфигурационные константы для настройки и инициализации инфраструктуры базы данных.
    /// </summary>
    public static class DatabaseConstants
    {
        /// <summary>
        /// Максимальное количество попыток подключения к СУБД при старте приложения в Docker контейнере.
        /// </summary>
        public const int MaxMigrationRetries = 6;

        /// <summary>
        /// Время ожидания (в секундах) между повторными попытками проверки доступности СУБД.
        /// </summary>
        public const int MigrationDelaySeconds = 5;

        /// <summary>
        /// Уникальный идентификатор (GUID) для первоначального тестового пользователя, создаваемого при миграции.
        /// Используется для обеспечения детерминированности связей в механизме Data Seeding.
        /// </summary>
        public const string TestUserId = "a18be34b-4b13-4336-bf45-d4197c234a7d";

        /// <summary>
        /// Уникальный идентификатор (GUID) для базовой роли тестового пользователя, создаваемой при миграции.
        /// Используется для обеспечения детерминированности связей в механизме Data Seeding.
        /// </summary>
        public const string TestRoleId = "8d4e123b-3121-477d-94c4-7e8c1b124a9f";
    }
}
