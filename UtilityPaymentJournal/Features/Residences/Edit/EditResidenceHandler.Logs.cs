
namespace UtilityPaymentJournal.Features.Residences.Edit
{
    public partial class EditResidenceHandler
    {
        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Information,
            Message = "Запрос на обновление объекта недвижимости в БД. ID записи: {id}. Новый адрес: {address}")]
        private static partial void LogResidenceUpdateRequested(ILogger<EditResidenceHandler> logger, long id, string address);

        [LoggerMessage(
            EventId = 2011,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: объект недвижимости с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<EditResidenceHandler> logger, long id);

        [LoggerMessage(
            EventId = 2005,
            Level = LogLevel.Information,
            Message = "Объект недвижимости с ID: {id} успешно изменен в БД")]
        private static partial void LogResidenceUpdatedInDb(ILogger<EditResidenceHandler> logger, long id);
    }
}
