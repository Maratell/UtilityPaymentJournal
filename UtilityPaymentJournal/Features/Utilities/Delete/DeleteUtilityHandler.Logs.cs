
namespace UtilityPaymentJournal.Features.Utilities.Delete
{
    public partial class DeleteUtilityHandler
    {
        [LoggerMessage(
            EventId = 2203,
            Level = LogLevel.Information,
            Message = "Запрос на удаление коммунальной услуги из БД. ID записи: {id}")]
        private static partial void LogUtilityDeletionRequested(ILogger<DeleteUtilityHandler> logger, long id);

        [LoggerMessage(
            EventId = 2211,
            Level = LogLevel.Warning,
            Message = "Операция изменения или удаления прервана: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<DeleteUtilityHandler> logger, long id);

        [LoggerMessage(
            EventId = 2206,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга с ID: {id} успешно удалено из БД")]
        private static partial void LogUtilityDeletedFromDb(ILogger<DeleteUtilityHandler> logger, long id);
    }
}
