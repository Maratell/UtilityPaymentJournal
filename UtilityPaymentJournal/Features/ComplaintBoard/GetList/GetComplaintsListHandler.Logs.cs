
namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    public partial class GetComplaintsListHandler
    {
        [LoggerMessage(
            EventId = 2521,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех жалоб")]
        private static partial void LogFetchingAllComplaintsFromDb(ILogger<GetComplaintsListHandler> logger);

        [LoggerMessage(
            EventId = 2522,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей жалоб")]
        private static partial void LogFetchedAllComplaintsFromDbCount(ILogger<GetComplaintsListHandler> logger, int count);
    }
}
