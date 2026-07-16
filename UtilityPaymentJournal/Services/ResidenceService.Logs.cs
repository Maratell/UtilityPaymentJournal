using UtilityPaymentJournal.DTOs.Residences;

namespace UtilityPaymentJournal.Services
{
    public partial class ResidenceService
    {
        #region Начало выполнения операций (Уровень Debug/Information)

        [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "Запрос на создание жилого объекта в БД. Адрес: {address}")]
        private static partial void LogResidenceCreationRequested(ILogger<ResidenceService> logger, string address);

        [LoggerMessage(EventId = 2002, Level = LogLevel.Information, Message = "Запрос на обновление жилого объекта в БД. ID записи: {id}. Новый адрес: {address}")]
        private static partial void LogResidenceUpdateRequested(ILogger<ResidenceService> logger, long id, string address);

        [LoggerMessage(EventId = 2003, Level = LogLevel.Information, Message = "Запрос на удаление жилого объекта из БД. ID записи: {id}")]
        private static partial void LogResidenceDeletionRequested(ILogger<ResidenceService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(EventId = 2004, Level = LogLevel.Information, Message = "Жилой объект успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogResidenceCreatedInDb(ILogger<ResidenceService> logger, long id);

        [LoggerMessage(EventId = 2005, Level = LogLevel.Information, Message = "Жилой объект с ID: {id} успешно обновлен в БД")]
        private static partial void LogResidenceUpdatedInDb(ILogger<ResidenceService> logger, long id);

        [LoggerMessage(EventId = 2006, Level = LogLevel.Information, Message = "Жилой объект с ID: {id} успешно удален из БД")]
        private static partial void LogResidenceDeletedFromDb(ILogger<ResidenceService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod)

        [LoggerMessage(EventId = 2007, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех жилых объектов")]
        private static partial void LogFetchingAllResidencesFromDb(ILogger<ResidenceService> logger);

        [LoggerMessage(EventId = 2008, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей жилых объектов")]
        private static partial void LogFetchedAllResidencesFromDbCount(ILogger<ResidenceService> logger, int count);

        [LoggerMessage(EventId = 2009, Level = LogLevel.Debug, Message = "Запрос к БД на получение жилого объекта по ID: {id}")]
        private static partial void LogFetchingResidenceByIdFromDb(ILogger<ResidenceService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2010, Level = LogLevel.Warning, Message = "Операция отменена: жилой объект с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<ResidenceService> logger, long id);

        #endregion
    }
}
