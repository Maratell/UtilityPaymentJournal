
namespace UtilityPaymentJournal.Features.Utilities.Create
{
    public partial class CreateUtilityHandler
    {
        [LoggerMessage(
            EventId = 2201,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение коммунальной услуги в БД. Наименование: {name}")]
        private static partial void LogUtilityCreationRequested(ILogger<CreateUtilityHandler> logger, string name);

        [LoggerMessage(
            EventId = 2204,
            Level = LogLevel.Information,
            Message = "Коммунальная услуга успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityCreatedInDb(ILogger<CreateUtilityHandler> logger, long id);
    }
}
