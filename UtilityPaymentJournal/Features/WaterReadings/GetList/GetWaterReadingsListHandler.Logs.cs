
namespace UtilityPaymentJournal.Features.WaterReadings.GetList
{
    public partial class GetWaterReadingsListHandler
    {
        [LoggerMessage(
            EventId = 2307,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех показаний счетчиков воды")]
        private static partial void LogFetchingAllWaterReadingsFromDb(ILogger<GetWaterReadingsListHandler> logger);

        [LoggerMessage(
            EventId = 2308,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей показаний счетчиков воды")]
        private static partial void LogFetchedAllWaterReadingsFromDbCount(ILogger<GetWaterReadingsListHandler> logger, int count);
    }
}
