namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class ComplaintsApiController
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 1508,
            Level = LogLevel.Information,
            Message = "Запрос на добавление новой жалобы для услуги (UtilityId): {utilityId}. Заголовок: {title}")]
        private static partial void LogComplaintCreationRequested(ILogger<ComplaintsApiController> logger, long utilityId, string title);

        [LoggerMessage(
            EventId = 1509,
            Level = LogLevel.Information,
            Message = "Запрос на изменение жалобы. ID записи: {id}. Привязанная услуга (UtilityId): {utilityId}. Новый заголовок: {title}")]
        private static partial void LogComplaintUpdateRequested(ILogger<ComplaintsApiController> logger, long id, long utilityId, string title);

        [LoggerMessage(
            EventId = 1510,
            Level = LogLevel.Information,
            Message = "Запрос на удаление жалобы из системы. ID записи: {id}")]
        private static partial void LogComplaintDeletionRequested(ILogger<ComplaintsApiController> logger, long id);

        [LoggerMessage(
            EventId = 1511,
            Level = LogLevel.Information,
            Message = "Запрос на частичное обновление статуса жалобы с ID: {id}. Новый статус: {status}")]
        private static partial void LogComplaintStatusUpdateRequested(ILogger<ComplaintsApiController> logger, long id, int status);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1501, Level = LogLevel.Information, Message = "Жалоба с ID: {id} успешно удалена из системы")]
        private static partial void LogComplaintDeleted(ILogger<ComplaintsApiController> logger, long id);

        [LoggerMessage(EventId = 1502, Level = LogLevel.Information, Message = "Добавлена новая жалоба. Записи присвоен ID: {id}")]
        private static partial void LogComplaintCreated(ILogger<ComplaintsApiController> logger, long id);

        [LoggerMessage(EventId = 1503, Level = LogLevel.Information, Message = "Успешно обновлена жалоба с ID: {id}")]
        private static partial void LogComplaintUpdated(ILogger<ComplaintsApiController> logger, long id);

        [LoggerMessage(EventId = 1506, Level = LogLevel.Information, Message = "Успешно изменен статус жалобы с ID: {id} на статус: {status}")]
        private static partial void LogComplaintStatusUpdated(ILogger<ComplaintsApiController> logger, long id, int status);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1504, Level = LogLevel.Debug, Message = "Запрос на получение списка всех жалоб")]
        private static partial void LogFetchingAllComplaints(ILogger<ComplaintsApiController> logger);

        [LoggerMessage(EventId = 1505, Level = LogLevel.Debug, Message = "Извлечено {count} записей жалоб для отображения")]
        private static partial void LogFetchedAllComplaintsCount(ILogger<ComplaintsApiController> logger, int count);

        #endregion
    }
}
