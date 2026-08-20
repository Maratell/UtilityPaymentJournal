namespace UtilityPaymentJournal.Features.UtilityProviders.GetById
{
    public partial class GetUtilityProviderByIdHandler
    {
        [LoggerMessage(
            EventId = 2121,
            Level = LogLevel.Information,
            Message = "Запрос на получение данных поставщика коммунальных услуг из БД. ID записи: {id}")]
        private static partial void LogFetchingUtilityProviderByIdFromDb(ILogger<GetUtilityProviderByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2131,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: поставщик коммунальных услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<GetUtilityProviderByIdHandler> logger, long id);
    }
}
