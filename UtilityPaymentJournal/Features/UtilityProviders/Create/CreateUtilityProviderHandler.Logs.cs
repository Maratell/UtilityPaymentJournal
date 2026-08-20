
namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    public partial class CreateUtilityProviderHandler
    {
        [LoggerMessage(
            EventId = 2101,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение поставщика коммунальных услуг в БД. Наименование: {name}")]
        private static partial void LogUtilityProviderCreationRequested(ILogger<CreateUtilityProviderHandler> logger, string name);

        [LoggerMessage(
            EventId = 2104,
            Level = LogLevel.Information,
            Message = "Поставщик коммунальных услуг успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityProviderCreatedInDb(ILogger<CreateUtilityProviderHandler> logger, long id);
    }
}
