namespace UtilityPaymentJournal.Features.UtilityProviders.Queries
{
    public partial class UtilityProviderQueryService
    {
        #region Процесс получения данных (Уровень Information)

        [LoggerMessage(
            EventId = 2121,
            Level = LogLevel.Information,
            Message = "Запрос на получение данных поставщика коммунальных услуг из БД. ID записи: {id}")]
        private static partial void LogFetchingUtilityProviderByIdFromDb(ILogger<UtilityProviderQueryService> logger, long id);

        [LoggerMessage(
            EventId = 2122,
            Level = LogLevel.Information,
            Message = "Запрос на получение списка всех поставщиков коммунальных услуг из БД")]
        private static partial void LogFetchingAllUtilityProvidersFromDb(ILogger<UtilityProviderQueryService> logger);

        [LoggerMessage(
            EventId = 2123,
            Level = LogLevel.Information,
            Message = "Успешно получено поставщиков коммунальных услуг из БД. Количество: {count}")]
        private static partial void LogFetchedAllUtilityProvidersFromDbCount(ILogger<UtilityProviderQueryService> logger, int count);

        #endregion

        #region Ошибки извлечения данных (Уровень Warning)

        [LoggerMessage(
            EventId = 2131,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: поставщик коммунальных услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<UtilityProviderQueryService> logger, long id);

        #endregion
    }
}
