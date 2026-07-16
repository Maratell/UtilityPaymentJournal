namespace UtilityPaymentJournal.Services
{
    public partial class UtilityProviderService
    {
        #region Начало выполнения операций (Уровень Information) ---

        [LoggerMessage(EventId = 3001, Level = LogLevel.Information, Message = "Запрос на создание поставщика услуг в БД. Название: {name}")]
        private static partial void LogUtilityProviderCreationRequested(ILogger<UtilityProviderService> logger, string name);

        [LoggerMessage(EventId = 3002, Level = LogLevel.Information, Message = "Запрос на обновление поставщика услуг {id} в БД. Новое название: {name}")]
        private static partial void LogUtilityProviderUpdateRequested(ILogger<UtilityProviderService> logger, long id, string name);

        [LoggerMessage(EventId = 3003, Level = LogLevel.Information, Message = "Запрос на удаление поставщика услуг из БД {id}")]
        private static partial void LogUtilityProviderDeletionRequested(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information) ---

        [LoggerMessage(EventId = 3004, Level = LogLevel.Information, Message = "Поставщик услуг {id} успешно сохранен в БД")]
        private static partial void LogUtilityProviderCreatedInDb(ILogger<UtilityProviderService> logger, long id);

        [LoggerMessage(EventId = 3005, Level = LogLevel.Information, Message = "Поставщик услуг {id} успешно изменен в БД")]
        private static partial void LogUtilityProviderUpdatedInDb(ILogger<UtilityProviderService> logger, long id);

        [LoggerMessage(EventId = 3006, Level = LogLevel.Information, Message = "Поставщик услуг {id} успешно удален из БД")]
        private static partial void LogUtilityProviderDeletedFromDb(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod) ---

        [LoggerMessage(EventId = 3007, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех поставщиков услуг")]
        private static partial void LogFetchingAllUtilityProvidersFromDb(ILogger<UtilityProviderService> logger);

        [LoggerMessage(EventId = 3008, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей поставщиков услуг")]
        private static partial void LogFetchedAllUtilityProvidersFromDbCount(ILogger<UtilityProviderService> logger, int count);

        [LoggerMessage(EventId = 3009, Level = LogLevel.Debug, Message = "Запрос к БД на получение поставщика услуг по ID {id}")]
        private static partial void LogFetchingUtilityProviderByIdFromDb(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning) ---

        [LoggerMessage(EventId = 3010, Level = LogLevel.Warning, Message = "Операция отменена: поставщик услуг {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<UtilityProviderService> logger, long id);

        #endregion
    }
}
