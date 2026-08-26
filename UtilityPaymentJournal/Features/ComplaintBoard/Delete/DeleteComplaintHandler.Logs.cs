
namespace UtilityPaymentJournal.Features.ComplaintBoard.Delete
{
    public partial class DeleteComplaintHandler
    {
        [LoggerMessage(
            EventId = 2504,
            Level = LogLevel.Information,
            Message = "Запрос на удаление жалобы из БД. ID записи: {id}")]
        private static partial void LogComplaintDeletionRequested(ILogger<DeleteComplaintHandler> logger, long id);

        [LoggerMessage(
            EventId = 2511,
            Level = LogLevel.Warning,
            Message = "Операция прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<DeleteComplaintHandler> logger, long id);

        [LoggerMessage(
           EventId = 2508,
           Level = LogLevel.Information,
           Message = "Жалоба с ID: {id} успешно удалена из БД")]
        private static partial void LogComplaintDeletedFromDb(ILogger<DeleteComplaintHandler> logger, long id);
    }
}
