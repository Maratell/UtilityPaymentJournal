namespace UtilityPaymentJournal.Features.ElectricityReadings.Queries
{
    /// <summary>
    /// Partial-класс логов для сервиса запросов показаний счетчиков электроэнергии.
    /// Содержит высокопроизводительные методы логирования операций чтения.
    /// </summary>
    public partial class ElectricityReadingQueryService
    {
        #region Чтение данных (Уровень Debug)

        [LoggerMessage(
            EventId = 2407,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех показаний счетчиков электроэнергии")]
        private static partial void LogFetchingAllElectricityReadingsFromDb(ILogger<ElectricityReadingQueryService> logger);

        [LoggerMessage(
            EventId = 2408,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей показаний счетчиков электроэнергии")]
        private static partial void LogFetchedAllElectricityReadingsFromDbCount(ILogger<ElectricityReadingQueryService> logger, int count);

        [LoggerMessage(
            EventId = 2409,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение показания счетчика электроэнергии по ID: {id}")]
        private static partial void LogFetchingElectricityReadingByIdFromDb(ILogger<ElectricityReadingQueryService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2410,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<ElectricityReadingQueryService> logger, long id);

        #endregion
    }
}
