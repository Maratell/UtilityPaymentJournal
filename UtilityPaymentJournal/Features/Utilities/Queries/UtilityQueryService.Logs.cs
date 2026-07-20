namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// Partial-класс логов для сервиса запросов коммунальных услуг.
    /// Содержит высокопроизводительные методы логирования операций чтения.
    /// </summary>
    public partial class UtilityQueryService
    {
        #region Чтение данных (Уровень Debug)

        [LoggerMessage(
            EventId = 2207,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilitiesFromDb(ILogger<UtilityQueryService> logger);

        [LoggerMessage(
            EventId = 2208,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей коммунальных услуг")]
        private static partial void LogFetchedAllUtilitiesFromDbCount(ILogger<UtilityQueryService> logger, int count);

        [LoggerMessage(
            EventId = 2209,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение коммунальной услуги по ID: {id}")]
        private static partial void LogFetchingUtilityByIdFromDb(ILogger<UtilityQueryService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2210,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<UtilityQueryService> logger, long id);

        #endregion
    }
}
