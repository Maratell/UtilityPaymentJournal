
namespace UtilityPaymentJournal.Features.ElectricityReadings.GetList
{
    public partial class GetElectricityReadingsListHandler
    {
        [LoggerMessage(
            EventId = 2407,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех показаний счетчиков электроэнергии")]
        private static partial void LogFetchingAllElectricityReadingsFromDb(ILogger<GetElectricityReadingsListHandler> logger);

        [LoggerMessage(
            EventId = 2408,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей показаний счетчиков электроэнергии")]
        private static partial void LogFetchedAllElectricityReadingsFromDbCount(ILogger<GetElectricityReadingsListHandler> logger, int count);
    }
}
