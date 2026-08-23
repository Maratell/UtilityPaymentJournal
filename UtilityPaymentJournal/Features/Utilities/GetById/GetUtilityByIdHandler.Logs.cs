
namespace UtilityPaymentJournal.Features.Utilities.GetById
{
    public partial class GetUtilityByIdHandler
    {
        [LoggerMessage(
            EventId = 2209,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение коммунальной услуги по ID: {id}")]
        private static partial void LogFetchingUtilityByIdFromDb(ILogger<GetUtilityByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2210,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<GetUtilityByIdHandler> logger, long id);
    }
}
