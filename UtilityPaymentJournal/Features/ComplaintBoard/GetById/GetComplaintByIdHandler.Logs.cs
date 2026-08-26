
namespace UtilityPaymentJournal.Features.ComplaintBoard.GetById
{
    public partial class GetComplaintByIdHandler
    {
        [LoggerMessage(
            EventId = 2523,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение жалобы по ID: {id}")]
        private static partial void LogFetchingComplaintByIdFromDb(ILogger<GetComplaintByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2531,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<GetComplaintByIdHandler> logger, long id);
    }
}
