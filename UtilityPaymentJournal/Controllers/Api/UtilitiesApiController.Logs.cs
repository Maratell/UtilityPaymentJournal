namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class UtilitiesApiController
    {
        #region Начало выполнения операций (Уровень Information) ---

        [LoggerMessage(EventId = 1208, Level = LogLevel.Information, Message = "Запрос на создание коммунальной услуги. Название: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<UtilitiesApiController> logger, string name);

        [LoggerMessage(EventId = 1209, Level = LogLevel.Information, Message = "Запрос на обновление коммунальной услуги. ID записи: {id}. Новое название: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<UtilitiesApiController> logger, long id, string name);

        [LoggerMessage(EventId = 1210, Level = LogLevel.Information, Message = "Запрос на удаление коммунальной услуги. ID записи: {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<UtilitiesApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information) ---

        [LoggerMessage(EventId = 1201, Level = LogLevel.Information, Message = "Коммунальная услуга с ID: {id} успешно удалена из системы")]
        private static partial void LogUtilityDeleted(ILogger<UtilitiesApiController> logger, long id);

        [LoggerMessage(EventId = 1202, Level = LogLevel.Information, Message = "Создана коммунальная услуга. Записи присвоен ID: {id}")]
        private static partial void LogUtilityCreated(ILogger<UtilitiesApiController> logger, long id);

        [LoggerMessage(EventId = 1203, Level = LogLevel.Information, Message = "Успешно обновлена коммунальная услуга с ID: {id}")]
        private static partial void LogUtilityUpdated(ILogger<UtilitiesApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Seq) ---

        [LoggerMessage(EventId = 1204, Level = LogLevel.Debug, Message = "Запрос на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilities(ILogger<UtilitiesApiController> logger);

        [LoggerMessage(EventId = 1205, Level = LogLevel.Debug, Message = "Извлечено {count} коммунальных услуг для отображения")]
        private static partial void LogFetchedAllUtilitiesCount(ILogger<UtilitiesApiController> logger, int count);

        #endregion
    }
}
