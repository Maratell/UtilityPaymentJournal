namespace UtilityPaymentJournal.Services
{
    public partial class UtilityService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 2201, Level = LogLevel.Information, Message = "Запрос на создание коммунальной услуги в БД. Название: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<UtilityService> logger, string name);

        [LoggerMessage(EventId = 2202, Level = LogLevel.Information, Message = "Запрос на обновление коммунальной услуги в БД. ID записи: {id}. Новое название: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<UtilityService> logger, long id, string name);

        [LoggerMessage(EventId = 2203, Level = LogLevel.Information, Message = "Запрос на удаление коммунальной услуги из БД. ID записи: {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<UtilityService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(EventId = 2204, Level = LogLevel.Information, Message = "Коммунальная услуга успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityCreatedInDb(ILogger<UtilityService> logger, long id);

        [LoggerMessage(EventId = 2205, Level = LogLevel.Information, Message = "Коммунальная услуга с ID: {id} успешно обновлена в БД")]
        private static partial void LogUtilityUpdatedInDb(ILogger<UtilityService> logger, long id);

        [LoggerMessage(EventId = 2206, Level = LogLevel.Information, Message = "Коммунальная услуга с ID: {id} успешно удалена из БД")]
        private static partial void LogUtilityDeletedFromDb(ILogger<UtilityService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod)

        [LoggerMessage(EventId = 2207, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilitiesFromDb(ILogger<UtilityService> logger);

        [LoggerMessage(EventId = 2208, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей коммунальных услуг")]
        private static partial void LogFetchedAllUtilitiesFromDbCount(ILogger<UtilityService> logger, int count);

        [LoggerMessage(EventId = 2209, Level = LogLevel.Debug, Message = "Запрос к БД на получение коммунальной услуги по ID: {id}")]
        private static partial void LogFetchingUtilityByIdFromDb(ILogger<UtilityService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2210, Level = LogLevel.Warning, Message = "Операция отменена: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<UtilityService> logger, long id);

        #endregion
    }
}
