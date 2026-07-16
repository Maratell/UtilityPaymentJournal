namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class UtilitiesApiController
    {
        #region Начало выполнения операций (Уровень Information) ---

        [LoggerMessage(EventId = 6008, Level = LogLevel.Information, Message = "Запрос на создание коммунальной услуги. Название: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<UtilitiesApiController> logger, string name);

        [LoggerMessage(EventId = 6009, Level = LogLevel.Information, Message = "Запрос на обновление коммунальной услуги {id}. Новое название: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<UtilitiesApiController> logger, long id, string name);

        [LoggerMessage(EventId = 6010, Level = LogLevel.Information, Message = "Запрос на удаление коммунальной услуги {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<UtilitiesApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information) ---

        [LoggerMessage(EventId = 6001, Level = LogLevel.Information, Message = "Услуга {id} успешно удалена из системы")]
        private static partial void LogUtilityDeleted(ILogger<UtilitiesApiController> logger, long id);

        [LoggerMessage(EventId = 6002, Level = LogLevel.Information, Message = "Создана коммунальная услуга {id}")]
        private static partial void LogUtilityCreated(ILogger<UtilitiesApiController> logger, long id);

        [LoggerMessage(EventId = 6003, Level = LogLevel.Information, Message = "Обновлена коммунальная услуга {id}")]
        private static partial void LogUtilityUpdated(ILogger<UtilitiesApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Seq) ---

        [LoggerMessage(EventId = 6004, Level = LogLevel.Debug, Message = "Запрос на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilities(ILogger<UtilitiesApiController> logger);

        [LoggerMessage(EventId = 6005, Level = LogLevel.Debug, Message = "Извлечено {count} коммунальных услуг для отображения")]
        private static partial void LogFetchedAllUtilitiesCount(ILogger<UtilitiesApiController> logger, int count);

        #endregion
    }
}
