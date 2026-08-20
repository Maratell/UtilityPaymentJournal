
namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    public partial class EditUtilityProviderHandler
    {
        [LoggerMessage(
            EventId = 2102,
            Level = LogLevel.Information,
            Message = "Запрос на обновление поставщика коммунальных услуг в БД. ID записи: {id}. Новое наименование: {name}")]
        private static partial void LogUtilityProviderUpdateRequested(ILogger<EditUtilityProviderHandler> logger, long id, string name);

        [LoggerMessage(
            EventId = 2111,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: поставщик коммунальных услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<EditUtilityProviderHandler> logger, long id);

        [LoggerMessage(
            EventId = 2105,
            Level = LogLevel.Information,
            Message = "Поставщик коммунальных услуг с ID: {id} успешно изменен в БД")]
        private static partial void LogUtilityProviderUpdatedInDb(ILogger<EditUtilityProviderHandler> logger, long id);
    }
}
