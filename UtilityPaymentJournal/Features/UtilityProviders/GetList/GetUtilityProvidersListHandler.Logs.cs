namespace UtilityPaymentJournal.Features.UtilityProviders.GetList
{
    public partial class GetUtilityProvidersListHandler
    {
        [LoggerMessage(
            EventId = 2122,
            Level = LogLevel.Information,
            Message = "Запрос на получение списка всех поставщиков коммунальных услуг из БД")]
        private static partial void LogFetchingAllUtilityProvidersFromDb(ILogger<GetUtilityProvidersListHandler> logger);

        [LoggerMessage(
            EventId = 2123,
            Level = LogLevel.Information,
            Message = "Успешно получено поставщиков коммунальных услуг из БД. Количество: {count}")]
        private static partial void LogFetchedAllUtilityProvidersFromDbCount(ILogger<GetUtilityProvidersListHandler> logger, int count);
    }
}
