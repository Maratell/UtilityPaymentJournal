namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class WaterReadingsApiController
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 1308, Level = LogLevel.Information, Message = "Запрос на добавление показания счетчика воды. Значение: {currentValue}")]
        private static partial void LogWaterReadingCreationRequested(ILogger<WaterReadingsApiController> logger, long currentValue);

        [LoggerMessage(EventId = 1309, Level = LogLevel.Information, Message = "Запрос на изменение показания счетчика воды. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogWaterReadingUpdateRequested(ILogger<WaterReadingsApiController> logger, long id, long currentValue);

        [LoggerMessage(EventId = 1310, Level = LogLevel.Information, Message = "Запрос на удаление показания счетчика воды. ID записи: {id}")]
        private static partial void LogWaterReadingDeletionRequested(ILogger<WaterReadingsApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1301, Level = LogLevel.Information, Message = "Показание счетчика воды с ID: {id} успешно удалено из системы")]
        private static partial void LogWaterReadingDeleted(ILogger<WaterReadingsApiController> logger, long id);

        [LoggerMessage(EventId = 1302, Level = LogLevel.Information, Message = "Добавлено новое показание счетчика воды. Записи присвоен ID: {id}")]
        private static partial void LogWaterReadingCreated(ILogger<WaterReadingsApiController> logger, long id);

        [LoggerMessage(EventId = 1303, Level = LogLevel.Information, Message = "Обновлено показание счетчика воды с ID: {id}")]
        private static partial void LogUtilityProviderUpdated(ILogger<WaterReadingsApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1304, Level = LogLevel.Debug, Message = "Запрос на получение списка всех показаний счетчиков воды")]
        private static partial void LogFetchingAllWaterReadings(ILogger<WaterReadingsApiController> logger);

        [LoggerMessage(EventId = 1305, Level = LogLevel.Debug, Message = "Извлечено {count} записей показаний счетчиков воды для отображения")]
        private static partial void LogFetchedAllWaterReadingsCount(ILogger<WaterReadingsApiController> logger, int count);

        #endregion
    }
}
