namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class ElectricityReadingsApiController
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 1408, Level = LogLevel.Information, Message = "Запрос на добавление показания счетчика электроэнергии. Значение: {currentValue}")]
        private static partial void LogElectricityReadingCreationRequested(ILogger<ElectricityReadingsApiController> logger, long currentValue);

        [LoggerMessage(EventId = 1409, Level = LogLevel.Information, Message = "Запрос на изменение показания счетчика электроэнергии. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogElectricityReadingUpdateRequested(ILogger<ElectricityReadingsApiController> logger, long id, long currentValue);

        [LoggerMessage(EventId = 1410, Level = LogLevel.Information, Message = "Запрос на удаление показания счетчика электроэнергии. ID записи: {id}")]
        private static partial void LogElectricityReadingDeletionRequested(ILogger<ElectricityReadingsApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1401, Level = LogLevel.Information, Message = "Показание счетчика электроэнергии с ID: {id} успешно удалено из системы")]
        private static partial void LogElectricityReadingDeleted(ILogger<ElectricityReadingsApiController> logger, long id);

        [LoggerMessage(EventId = 1402, Level = LogLevel.Information, Message = "Добавлено новое показание счетчика электроэнергии. Записи присвоен ID: {id}")]
        private static partial void LogElectricityReadingCreated(ILogger<ElectricityReadingsApiController> logger, long id);

        [LoggerMessage(EventId = 1403, Level = LogLevel.Information, Message = "Успешно обновлено показание счетчика электроэнергии с ID: {id}")]
        private static partial void LogElectricityReadingUpdated(ILogger<ElectricityReadingsApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1404, Level = LogLevel.Debug, Message = "Запрос на получение списка всех показаний счетчиков электроэнергии")]
        private static partial void LogFetchingAllElectricityReadings(ILogger<ElectricityReadingsApiController> logger);

        [LoggerMessage(EventId = 1405, Level = LogLevel.Debug, Message = "Извлечено {count} записей показаний счетчиков электроэнергии для отображения")]
        private static partial void LogFetchedAllElectricityReadingsCount(ILogger<ElectricityReadingsApiController> logger, int count);

        #endregion
    }
}
