namespace UtilityPaymentJournal.Services
{
    public partial class ElectricityReadingService
    {
        #region Начало выполнения операций (Уровень Information) ---

        [LoggerMessage(EventId = 2401, Level = LogLevel.Information, Message = "Запрос на сохранение показания счетчика электроэнергии в БД. Значение: {currentValue}")]
        private static partial void LogElectricityReadingCreationRequested(ILogger<ElectricityReadingService> logger, long currentValue);

        [LoggerMessage(EventId = 2402, Level = LogLevel.Information, Message = "Запрос на обновление показания счетчика электроэнергии в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogElectricityReadingUpdateRequested(ILogger<ElectricityReadingService> logger, long id, long currentValue);

        [LoggerMessage(EventId = 2403, Level = LogLevel.Information, Message = "Запрос на удаление показания счетчика электроэнергии из БД. ID записи: {id}")]
        private static partial void LogElectricityReadingDeletionRequested(ILogger<ElectricityReadingService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information) ---

        [LoggerMessage(EventId = 2404, Level = LogLevel.Information, Message = "Показание счетчика электроэнергии успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogElectricityReadingCreatedInDb(ILogger<ElectricityReadingService> logger, long id);

        [LoggerMessage(EventId = 2405, Level = LogLevel.Information, Message = "Показание счетчика электроэнергии с ID: {id} успешно изменено в БД")]
        private static partial void LogElectricityReadingUpdatedInDb(ILogger<ElectricityReadingService> logger, long id);

        [LoggerMessage(EventId = 2406, Level = LogLevel.Information, Message = "Показание счетчика электроэнергии с ID: {id} успешно удалено из БД")]
        private static partial void LogElectricityReadingDeletedFromDb(ILogger<ElectricityReadingService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod) ---

        [LoggerMessage(EventId = 2407, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех показаний счетчиков электроэнергии")]
        private static partial void LogFetchingAllElectricityReadingsFromDb(ILogger<ElectricityReadingService> logger);

        [LoggerMessage(EventId = 2408, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей показаний счетчиков электроэнергии")]
        private static partial void LogFetchedAllElectricityReadingsFromDbCount(ILogger<ElectricityReadingService> logger, int count);

        [LoggerMessage(EventId = 2409, Level = LogLevel.Debug, Message = "Запрос к БД на получение показания счетчика электроэнергии по ID: {id}")]
        private static partial void LogFetchingElectricityReadingByIdFromDb(ILogger<ElectricityReadingService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning) ---

        [LoggerMessage(EventId = 2410, Level = LogLevel.Warning, Message = "Операция отменена: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<ElectricityReadingService> logger, long id);

        #endregion
    }
}
