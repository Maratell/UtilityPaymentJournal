
namespace UtilityPaymentJournal.Features.Residences.Create
{
    public partial class CreateResidenceHandler
    {
        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение объекта недвижимости в БД. Адрес: {address}")]
        private static partial void LogResidenceCreationRequested(ILogger<CreateResidenceHandler> logger, string address);

        [LoggerMessage(
            EventId = 2004,
            Level = LogLevel.Information,
            Message = "Объект недвижимости успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogResidenceCreatedInDb(ILogger<CreateResidenceHandler> logger, long id);
    }
}
