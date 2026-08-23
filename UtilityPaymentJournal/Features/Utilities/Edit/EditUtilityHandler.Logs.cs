
namespace UtilityPaymentJournal.Features.Utilities.Edit
{
    public partial class EditUtilityHandler
    {
        [LoggerMessage(
            EventId = 2202,
            Level = LogLevel.Information,
            Message = "Запрос на обновление коммунальной услуги в БД. ID записи: {id}. Новое наименование: {name}")]
        private static partial void LogUtilityUpdateRequested(ILogger<EditUtilityHandler> logger, long id, string name);

        [LoggerMessage(
            EventId = 2211,
            Level = LogLevel.Warning,
            Message = "Операция изменения или удаления прервана: коммунальная услуга с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityNotFoundInDb(ILogger<EditUtilityHandler> logger, long id);

        [LoggerMessage(
            EventId = 2205,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга с ID: {id} успешно изменена в БД")]
        private static partial void LogUtilityUpdatedInDb(ILogger<EditUtilityHandler> logger, long id);
    }
}
