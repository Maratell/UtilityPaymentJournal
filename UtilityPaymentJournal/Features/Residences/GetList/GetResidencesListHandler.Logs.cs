
namespace UtilityPaymentJournal.Features.Residences.GetList
{
    public partial class GetResidencesListHandler
    {
        [LoggerMessage(
            EventId = 2022,
            Level = LogLevel.Information,
            Message = "Запрос на получение списка всех объектов недвижимости из БД")]
        private static partial void LogFetchingAllResidencesFromDb(ILogger<GetResidencesListHandler> logger);

        [LoggerMessage(
            EventId = 2023,
            Level = LogLevel.Information,
            Message = "Успешно получено объектов недвижимости из БД. Количество: {count}")]
        private static partial void LogFetchedAllResidencesFromDbCount(ILogger<GetResidencesListHandler> logger, int count);
    }
}
