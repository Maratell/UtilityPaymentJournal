
namespace UtilityPaymentJournal.Features.ComplaintBoard.Edit
{
    public partial class EditComplaintHandler
    {
        [LoggerMessage(
            EventId = 2502,
            Level = LogLevel.Information,
            Message = "Запрос на обновление жалобы в БД. ID записи: {id}. Новый заголовок: {title}")]
        private static partial void LogComplaintUpdateRequested(ILogger<EditComplaintHandler> logger, long id, string title);

        [LoggerMessage(
            EventId = 2511,
            Level = LogLevel.Warning,
            Message = "Операция прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<EditComplaintHandler> logger, long id);

        [LoggerMessage(
            EventId = 2506,
            Level = LogLevel.Information,
            Message = "Жалоба с ID: {id} успешно изменена в БД")]
        private static partial void LogComplaintUpdatedInDb(ILogger<EditComplaintHandler> logger, long id);
    }
}
