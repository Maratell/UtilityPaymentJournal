namespace UtilityPaymentJournal.Features.Residences.Commands
{
    public partial class ResidenceCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение объекта недвижимости в БД. Адрес: {address}")]
        private static partial void LogResidenceCreationRequested(ILogger<ResidenceCommandService> logger, string address);

        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Information,
            Message = "Запрос на обновление объекта недвижимости в БД. ID записи: {id}. Новый адрес: {address}")]
        private static partial void LogResidenceUpdateRequested(ILogger<ResidenceCommandService> logger, long id, string address);

        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Information,
            Message = "Запрос на удаление объекта недвижимости из БД. ID записи: {id}")]
        private static partial void LogResidenceDeletionRequested(ILogger<ResidenceCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2004,
            Level = LogLevel.Information,
            Message = "Объект недвижимости успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogResidenceCreatedInDb(ILogger<ResidenceCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2005,
            Level = LogLevel.Information,
            Message = "Объект недвижимости с ID: {id} успешно изменен в БД")]
        private static partial void LogResidenceUpdatedInDb(ILogger<ResidenceCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2006,
            Level = LogLevel.Information,
            Message = "Объект недвижимости с ID: {id} успешно удален из БД")]
        private static partial void LogResidenceDeletedFromDb(ILogger<ResidenceCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2011,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: объект недвижимости с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<ResidenceCommandService> logger, long id);

        #endregion
    }
}
