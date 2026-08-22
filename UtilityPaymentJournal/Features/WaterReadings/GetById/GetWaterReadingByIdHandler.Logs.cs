
namespace UtilityPaymentJournal.Features.WaterReadings.GetById
{
    public partial class GetWaterReadingByIdHandler
    {
        [LoggerMessage(
           EventId = 2309,
           Level = LogLevel.Debug,
           Message = "Запрос к БД на получение показания счетчика воды по ID: {id}")]
        private static partial void LogFetchingWaterReadingByIdFromDb(ILogger<GetWaterReadingByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2310,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<GetWaterReadingByIdHandler> logger, long id);
    }
}
