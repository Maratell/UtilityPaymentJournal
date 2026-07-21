namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class UtilityProvidersApiController
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 1108, Level = LogLevel.Information, Message = "Запрос на создание поставщика услуг. Название: {name}")]
        private static partial void LogUtilityProviderCreationRequested(ILogger<UtilityProvidersApiController> logger, string name);

        [LoggerMessage(EventId = 1109, Level = LogLevel.Information, Message = "Запрос на обновление поставщика услуг. ID записи: {id}. Новое название: {name}")]
        private static partial void LogUtilityProviderUpdateRequested(ILogger<UtilityProvidersApiController> logger, long id, string name);

        [LoggerMessage(EventId = 1110, Level = LogLevel.Information, Message = "Запрос на удаление поставщика услуг. ID записи: {id}")]
        private static partial void LogUtilityProviderDeletionRequested(ILogger<UtilityProvidersApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1101, Level = LogLevel.Information, Message = "Поставщик услуг с ID: {id} успешно удален из системы")]
        private static partial void LogUtilityProviderDeleted(ILogger<UtilityProvidersApiController> logger, long id);

        [LoggerMessage(EventId = 1102, Level = LogLevel.Information, Message = "Создан поставщик услуг. Записи присвоен ID: {id}")]
        private static partial void LogUtilityProviderCreated(ILogger<UtilityProvidersApiController> logger, long id);

        [LoggerMessage(EventId = 1103, Level = LogLevel.Information, Message = "Успешно обновлен поставщик услуг с ID: {id}")]
        private static partial void LogUtilityProviderUpdated(ILogger<UtilityProvidersApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1104, Level = LogLevel.Debug, Message = "Запрос на получение списка всех поставщиков услуг")]
        private static partial void LogFetchingAllUtilityProviders(ILogger<UtilityProvidersApiController> logger);

        [LoggerMessage(EventId = 1105, Level = LogLevel.Debug, Message = "Извлечено {count} поставщиков услуг для отображения")]
        private static partial void LogFetchedAllUtilityProvidersCount(ILogger<UtilityProvidersApiController> logger, int count);

        #endregion
    }
}
