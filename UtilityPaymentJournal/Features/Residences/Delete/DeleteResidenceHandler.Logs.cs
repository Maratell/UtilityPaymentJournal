
namespace UtilityPaymentJournal.Features.Residences.Delete
{
    public partial class DeleteResidenceHandler
    {
        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Information,
            Message = "Запрос на удаление объекта недвижимости из БД. ID записи: {id}")]
        private static partial void LogResidenceDeletionRequested(ILogger<DeleteResidenceHandler> logger, long id);

        [LoggerMessage(
            EventId = 2011,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: объект недвижимости с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<DeleteResidenceHandler> logger, long id);

        [LoggerMessage(
            EventId = 2006,
            Level = LogLevel.Information,
            Message = "Объект недвижимости с ID: {id} успешно удален из БД")]
        private static partial void LogResidenceDeletedFromDb(ILogger<DeleteResidenceHandler> logger, long id);
    }
}
