namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    public partial class UtilityCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2201,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение коммунальной услуги в БД. Наименование: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<UtilityCommandService> logger, string name);

        [LoggerMessage(
            EventId = 2202,
            Level = LogLevel.Information,
            Message = "Запрос на обновление коммунальной услуги в БД. ID записи: {id}. Новое наименование: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<UtilityCommandService> logger, long id, string name);

        [LoggerMessage(
            EventId = 2203,
            Level = LogLevel.Information,
            Message = "Запрос на удаление коммунальной услуги из БД. ID записи: {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<UtilityCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2204,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityCreatedInDb(ILogger<UtilityCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2205,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга с ID: {id} успешно изменена в БД")]
        private static partial void LogUtilityUpdatedInDb(ILogger<UtilityCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2206,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга с ID: {id} успешно удалено из БД")]
        private static partial void LogUtilityDeletedFromDb(ILogger<UtilityCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2211,
            Level = LogLevel.Warning,
            Message = "Операция изменения или удаления прервана: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<UtilityCommandService> logger, long id);

        #endregion
    }
}
