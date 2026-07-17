namespace UtilityPaymentJournal.Features.WaterReadings.Commands
{
    public partial class WaterReadingCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2301,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение показания счетчика воды в БД. Значение: {currentValue}")]
        private static partial void LogWaterReadingCreationRequested(ILogger<WaterReadingCommandService> logger, long currentValue);

        [LoggerMessage(
            EventId = 2302,
            Level = LogLevel.Information,
            Message = "Запрос на обновление показания счетчика воды в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogWaterReadingUpdateRequested(ILogger<WaterReadingCommandService> logger, long id, long currentValue);

        [LoggerMessage(
            EventId = 2303,
            Level = LogLevel.Information,
            Message = "Запрос на удаление показания счетчика воды из БД. ID записи: {id}")]
        private static partial void LogWaterReadingDeletionRequested(ILogger<WaterReadingCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2304,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogWaterReadingCreatedInDb(ILogger<WaterReadingCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2305,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды с ID: {id} успешно изменено в БД")]
        private static partial void LogWaterReadingUpdatedInDb(ILogger<WaterReadingCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2306,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды с ID: {id} успешно удалено из БД")]
        private static partial void LogWaterReadingDeletedFromDb(ILogger<WaterReadingCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2311,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<WaterReadingCommandService> logger, long id);

        #endregion
    }
}
