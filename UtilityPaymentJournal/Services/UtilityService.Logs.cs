namespace UtilityPaymentJournal.Services
{
    public partial class UtilityService
    {
        #region Начало выполнения операций (Уровень Information) ---

        [LoggerMessage(EventId = 5001, Level = LogLevel.Information, Message = "Запрос на создание коммунальной услуги в БД. Название: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<UtilityService> logger, string name);

        [LoggerMessage(EventId = 5002, Level = LogLevel.Information, Message = "Запрос на обновление коммунальной услуги {id} в БД. Новое название: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<UtilityService> logger, long id, string name);

        [LoggerMessage(EventId = 5003, Level = LogLevel.Information, Message = "Запрос на удаление коммунальной услуги из БД {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<UtilityService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information) ---

        [LoggerMessage(EventId = 5004, Level = LogLevel.Information, Message = "Коммунальная услуга {id} успешно сохранена в БД")]
        private static partial void LogUtilityCreatedInDb(ILogger<UtilityService> logger, long id);

        [LoggerMessage(EventId = 5005, Level = LogLevel.Information, Message = "Коммунальная услуга {id} успешно изменена в БД")]
        private static partial void LogUtilityUpdatedInDb(ILogger<UtilityService> logger, long id);

        [LoggerMessage(EventId = 5006, Level = LogLevel.Information, Message = "Коммунальная услуга {id} успешно удалена из БД")]
        private static partial void LogUtilityDeletedFromDb(ILogger<UtilityService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod) ---

        [LoggerMessage(EventId = 5007, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilitiesFromDb(ILogger<UtilityService> logger);

        [LoggerMessage(EventId = 5008, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей коммунальных услуг")]
        private static partial void LogFetchedAllUtilitiesFromDbCount(ILogger<UtilityService> logger, int count);

        [LoggerMessage(EventId = 5009, Level = LogLevel.Debug, Message = "Запрос к БД на получение коммунальной услуги по ID {id}")]
        private static partial void LogFetchingUtilityByIdFromDb(ILogger<UtilityService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning) ---

        [LoggerMessage(EventId = 5010, Level = LogLevel.Warning, Message = "Операция отменена: коммунальная услуга {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<UtilityService> logger, long id);

        #endregion
    }
}
