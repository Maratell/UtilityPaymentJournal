
namespace UtilityPaymentJournal.Features.ElectricityReadings.GetById
{
    public partial class GetElectricityReadingByIdHandler
    {
        [LoggerMessage(
            EventId = 2409,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение показания счетчика электроэнергии по ID: {id}")]
        private static partial void LogFetchingElectricityReadingByIdFromDb(ILogger<GetElectricityReadingByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2410,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<GetElectricityReadingByIdHandler> logger, long id);
    }
}
