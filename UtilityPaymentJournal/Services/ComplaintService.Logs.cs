namespace UtilityPaymentJournal.Services
{
    public partial class ComplaintService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2501,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение жалобы в БД для услуги (UtilityId): {utilityId}. Заголовок: {title}")]
        private static partial void LogComplaintCreationRequested(ILogger<ComplaintService> logger, long utilityId, string title);

        [LoggerMessage(
            EventId = 2502,
            Level = LogLevel.Information,
            Message = "Запрос на обновление жалобы в БД. ID записи: {id}. Привязанная услуга (UtilityId): {utilityId}. Новый заголовок: {title}")]
        private static partial void LogComplaintUpdateRequested(ILogger<ComplaintService> logger, long id, long utilityId, string title);

        [LoggerMessage(
            EventId = 2503,
            Level = LogLevel.Information,
            Message = "Запрос на удаление жалобы из БД. ID записи: {id}")]
        private static partial void LogComplaintDeletionRequested(ILogger<ComplaintService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(EventId = 2504, Level = LogLevel.Information, Message = "Жалоба успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogComplaintCreatedInDb(ILogger<ComplaintService> logger, long id);

        [LoggerMessage(EventId = 2505, Level = LogLevel.Information, Message = "Жалоба с ID: {id} успешно изменена в БД")]
        private static partial void LogComplaintUpdatedInDb(ILogger<ComplaintService> logger, long id);

        [LoggerMessage(EventId = 2506, Level = LogLevel.Information, Message = "Жалоба с ID: {id} успешно удалена из БД")]
        private static partial void LogComplaintDeletedFromDb(ILogger<ComplaintService> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 2507, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех жалоб")]
        private static partial void LogFetchingAllComplaintsFromDb(ILogger<ComplaintService> logger);

        [LoggerMessage(EventId = 2508, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {count} записей жалоб")]
        private static partial void LogFetchedAllComplaintsFromDbCount(ILogger<ComplaintService> logger, int count);

        [LoggerMessage(EventId = 2509, Level = LogLevel.Debug, Message = "Запрос к БД на получение жалобы по ID: {id}")]
        private static partial void LogFetchingComplaintByIdFromDb(ILogger<ComplaintService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2510, Level = LogLevel.Warning, Message = "Операция отменена: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<ComplaintService> logger, long id);

        #endregion
    }
}
