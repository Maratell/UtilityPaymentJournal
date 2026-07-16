namespace UtilityPaymentJournal.Services
{
    public partial class WaterReadingService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 2301, Level = LogLevel.Information, Message = "Запрос на сохранение показания счетчика воды в БД. Значение: {currentValue}")]
        private static partial void LogWaterReadingCreationRequested(ILogger<WaterReadingService> logger, long currentValue);

        [LoggerMessage(EventId = 2302, Level = LogLevel.Information, Message = "Запрос на обновление показания счетчика воды в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogWaterReadingUpdateRequested(ILogger<WaterReadingService> logger, long id, long currentValue);

        [LoggerMessage(EventId = 2303, Level = LogLevel.Information, Message = "Запрос на удаление показания счетчика воды из БД. ID записи: {id}")]
        private static partial void LogWaterReadingDeletionRequested(ILogger<WaterReadingService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(EventId = 2304, Level = LogLevel.Information, Message = "Показание счетчика воды успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogWaterReadingCreatedInDb(ILogger<WaterReadingService> logger, long id);

        [LoggerMessage(EventId = 2305, Level = LogLevel.Information, Message = "Показание счетчика воды с ID: {id} успешно изменено в БД")]
        private static partial void LogWaterReadingUpdatedInDb(ILogger<WaterReadingService> logger, long id);

        [LoggerMessage(EventId = 2306, Level = LogLevel.Information, Message = "Показание счетчика воды с ID: {id} успешно удалено из БД")]
        private static partial void LogWaterReadingDeletedFromDb(ILogger<WaterReadingService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod)

        [LoggerMessage(EventId = 2307, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех показаний счетчиков воды")]
        private static partial void LogFetchingAllWaterReadingsFromDb(ILogger<WaterReadingService> logger);

        [LoggerMessage(EventId = 2308, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей показаний счетчиков воды")]
        private static partial void LogFetchedAllWaterReadingsFromDbCount(ILogger<WaterReadingService> logger, int count);

        [LoggerMessage(EventId = 2309, Level = LogLevel.Debug, Message = "Запрос к БД на получение показания счетчика воды по ID: {id}")]
        private static partial void LogFetchingWaterReadingByIdFromDb(ILogger<WaterReadingService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2310, Level = LogLevel.Warning, Message = "Операция отменена: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<WaterReadingService> logger, long id);

        #endregion
    }
}
