
namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    public partial class CreateComplaintHandler
    {
        [LoggerMessage(
            EventId = 2501,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение жалобы в БД. Заголовок: {title}")]
        private static partial void LogComplaintCreationRequested(ILogger<CreateComplaintHandler> logger, string title);

        [LoggerMessage(
            EventId = 2505,
            Level = LogLevel.Information,
            Message = "Жалоба успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogComplaintCreatedInDb(ILogger<CreateComplaintHandler> logger, long id);
    }
}
