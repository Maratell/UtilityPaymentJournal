namespace UtilityPaymentJournal.Services
{
    public partial class UtilityProviderService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 2101, Level = LogLevel.Information, Message = "Запрос на создание поставщика услуг в БД. Название: {name}")]
        private static partial void LogUtilityProviderCreationRequested(ILogger<UtilityProviderService> logger, string name);

        [LoggerMessage(EventId = 2102, Level = LogLevel.Information, Message = "Запрос на обновление поставщика услуг в БД. ID записи: {id}. Новое название: {name}")]
        private static partial void LogUtilityProviderUpdateRequested(ILogger<UtilityProviderService> logger, long id, string name);

        [LoggerMessage(EventId = 2103, Level = LogLevel.Information, Message = "Запрос на удаление поставщика услуг из БД. ID записи: {id}")]
        private static partial void LogUtilityProviderDeletionRequested(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(EventId = 2104, Level = LogLevel.Information, Message = "Поставщик услуг успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityProviderCreatedInDb(ILogger<UtilityProviderService> logger, long id);

        [LoggerMessage(EventId = 2105, Level = LogLevel.Information, Message = "Поставщик услуг с ID: {id} успешно обновлен в БД")]
        private static partial void LogUtilityProviderUpdatedInDb(ILogger<UtilityProviderService> logger, long id);

        [LoggerMessage(EventId = 2106, Level = LogLevel.Information, Message = "Поставщик услуг с ID: {id} успешно удален из БД")]
        private static partial void LogUtilityProviderDeletedFromDb(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod)

        [LoggerMessage(EventId = 2107, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех поставщиков услуг")]
        private static partial void LogFetchingAllUtilityProvidersFromDb(ILogger<UtilityProviderService> logger);

        [LoggerMessage(EventId = 2108, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей поставщиков услуг")]
        private static partial void LogFetchedAllUtilityProvidersFromDbCount(ILogger<UtilityProviderService> logger, int count);

        [LoggerMessage(EventId = 2109, Level = LogLevel.Debug, Message = "Запрос к БД на получение поставщика услуг по ID: {id}")]
        private static partial void LogFetchingUtilityProviderByIdFromDb(ILogger<UtilityProviderService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2110, Level = LogLevel.Warning, Message = "Операция отменена: поставщик услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<UtilityProviderService> logger, long id);

        #endregion
    }
}
