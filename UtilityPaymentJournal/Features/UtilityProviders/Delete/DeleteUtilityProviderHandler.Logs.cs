
namespace UtilityPaymentJournal.Features.UtilityProviders.Delete
{
    public partial class DeleteUtilityProviderHandler
    {
        [LoggerMessage(
            EventId = 2103,
            Level = LogLevel.Information,
            Message = "Запрос на удаление поставщика коммунальных услуг из БД. ID записи: {id}")]
        private static partial void LogUtilityProviderDeletionRequested(ILogger<DeleteUtilityProviderHandler> logger, long id);

        [LoggerMessage(
            EventId = 2111,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: поставщик коммунальных услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<DeleteUtilityProviderHandler> logger, long id);

        [LoggerMessage(
           EventId = 2106,
           Level = LogLevel.Information,
           Message = "Поставщик коммунальных услуг с ID: {id} успешно удален из БД")]
        private static partial void LogUtilityProviderDeletedFromDb(ILogger<DeleteUtilityProviderHandler> logger, long id);
    }
}
