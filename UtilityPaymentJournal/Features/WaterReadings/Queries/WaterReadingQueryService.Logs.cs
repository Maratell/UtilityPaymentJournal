namespace UtilityPaymentJournal.Features.WaterReadings.Queries
{
    /// <summary>
    /// Partial-класс логов для сервиса запросов показаний счетчиков воды.
    /// Содержит высокопроизводительные методы логирования операций чтения.
    /// </summary>
    public partial class WaterReadingQueryService
    {
        #region Чтение данных (Уровень Debug)

        [LoggerMessage(
            EventId = 2307,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех показаний счетчиков воды")]
        private static partial void LogFetchingAllWaterReadingsFromDb(ILogger<WaterReadingQueryService> logger);

        [LoggerMessage(
            EventId = 2308,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей показаний счетчиков воды")]
        private static partial void LogFetchedAllWaterReadingsFromDbCount(ILogger<WaterReadingQueryService> logger, int count);

        [LoggerMessage(
            EventId = 2309,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение показания счетчика воды по ID: {id}")]
        private static partial void LogFetchingWaterReadingByIdFromDb(ILogger<WaterReadingQueryService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2310,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<WaterReadingQueryService> logger, long id);

        #endregion
    }
}
