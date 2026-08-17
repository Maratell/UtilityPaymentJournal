
namespace UtilityPaymentJournal.Features.Residences.GetById
{
    public partial class GetResidenceByIdHandler
    {
        [LoggerMessage(
            EventId = 2021,
            Level = LogLevel.Information,
            Message = "Запрос на получение данных объекта недвижимости из БД. ID записи: {id}")]
        private static partial void LogFetchingResidenceByIdFromDb(ILogger<GetResidenceByIdHandler> logger, long id);

        [LoggerMessage(
            EventId = 2031,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: объект недвижимости с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<GetResidenceByIdHandler> logger, long id);
    }
}
