
namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    public partial class GetUtilitiesListHandler
    {
        [LoggerMessage(
            EventId = 2207,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех коммунальных услуг")]
        private static partial void LogFetchingAllUtilitiesFromDb(ILogger<GetUtilitiesListHandler> logger);

        [LoggerMessage(
            EventId = 2208,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей коммунальных услуг")]
        private static partial void LogFetchedAllUtilitiesFromDbCount(ILogger<GetUtilitiesListHandler> logger, int count);
    }
}
