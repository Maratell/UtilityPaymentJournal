namespace UtilityPaymentJournal.Features.ElectricityReadings.Commands
{
    public partial class ElectricityReadingCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2401,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение показания счетчика электроэнергии в БД. Значение: {currentValue}")]
        private static partial void LogElectricityReadingCreationRequested(ILogger<ElectricityReadingCommandService> logger, long currentValue);

        [LoggerMessage(
            EventId = 2402,
            Level = LogLevel.Information,
            Message = "Запрос на обновление показания счетчика электроэнергии в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogElectricityReadingUpdateRequested(ILogger<ElectricityReadingCommandService> logger, long id, long currentValue);

        [LoggerMessage(
            EventId = 2403,
            Level = LogLevel.Information,
            Message = "Запрос на удаление показания счетчика электроэнергии из БД. ID записи: {id}")]
        private static partial void LogElectricityReadingDeletionRequested(ILogger<ElectricityReadingCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2404,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogElectricityReadingCreatedInDb(ILogger<ElectricityReadingCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2405,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии с ID: {id} успешно изменено в БД")]
        private static partial void LogElectricityReadingUpdatedInDb(ILogger<ElectricityReadingCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2406,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии с ID: {id} успешно удалено из БД")]
        private static partial void LogElectricityReadingDeletedFromDb(ILogger<ElectricityReadingCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2411,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<ElectricityReadingCommandService> logger, long id);

        #endregion
    }
}
