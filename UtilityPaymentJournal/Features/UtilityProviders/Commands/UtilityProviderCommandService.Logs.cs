namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    public partial class UtilityProviderCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2101,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение поставщика коммунальных услуг в БД. Наименование: {name}")]
        private static partial void LogUtilityProviderCreationRequested(ILogger<UtilityProviderCommandService> logger, string name);

        [LoggerMessage(
            EventId = 2102,
            Level = LogLevel.Information,
            Message = "Запрос на обновление поставщика коммунальных услуг в БД. ID записи: {id}. Новое наименование: {name}")]
        private static partial void LogUtilityProviderUpdateRequested(ILogger<UtilityProviderCommandService> logger, long id, string name);

        [LoggerMessage(
            EventId = 2103,
            Level = LogLevel.Information,
            Message = "Запрос на удаление поставщика коммунальных услуг из БД. ID записи: {id}")]
        private static partial void LogUtilityProviderDeletionRequested(ILogger<UtilityProviderCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2104,
            Level = LogLevel.Information,
            Message = "Поставщик коммунальных услуг успешно сохранен в БД. Записи присвоен ID: {id}")]
        private static partial void LogUtilityProviderCreatedInDb(ILogger<UtilityProviderCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2105,
            Level = LogLevel.Information,
            Message = "Поставщик коммунальных услуг с ID: {id} успешно изменен в БД")]
        private static partial void LogUtilityProviderUpdatedInDb(ILogger<UtilityProviderCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2106,
            Level = LogLevel.Information,
            Message = "Поставщик коммунальных услуг с ID: {id} успешно удален из БД")]
        private static partial void LogUtilityProviderDeletedFromDb(ILogger<UtilityProviderCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2111,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: поставщик коммунальных услуг с ID: {id} отсутствует в БД")]
        private static partial void LogUtilityProviderNotFoundInDb(ILogger<UtilityProviderCommandService> logger, long id);

        #endregion
    }
}
