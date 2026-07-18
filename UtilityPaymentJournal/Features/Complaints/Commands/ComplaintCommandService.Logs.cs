using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    public partial class ComplaintCommandService
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(
            EventId = 2501,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение жалобы в БД. Заголовок: {title}")]
        private static partial void LogComplaintCreationRequested(ILogger<ComplaintCommandService> logger, string title);

        [LoggerMessage(
            EventId = 2502,
            Level = LogLevel.Information,
            Message = "Запрос на обновление жалобы в БД. ID записи: {id}. Новый заголовок: {title}")]
        private static partial void LogComplaintUpdateRequested(ILogger<ComplaintCommandService> logger, long id, string title);

        [LoggerMessage(
            EventId = 2503,
            Level = LogLevel.Information,
            Message = "Запрос на точечное изменение статуса жалобы в БД. ID записи: {id}. Целевой статус: {newStatus}")]
        private static partial void LogComplaintStatusChangeRequested(ILogger<ComplaintCommandService> logger, long id, ComplaintStatus newStatus);

        [LoggerMessage(
            EventId = 2504,
            Level = LogLevel.Information,
            Message = "Запрос на удаление жалобы из БД. ID записи: {id}")]
        private static partial void LogComplaintDeletionRequested(ILogger<ComplaintCommandService> logger, long id);

        #endregion

        #region Успешный финал операций записи (Уровень Information)

        [LoggerMessage(
            EventId = 2505,
            Level = LogLevel.Information,
            Message = "Жалоба успешно сохранена в БД. Записи присвоен ID: {id}")]
        private static partial void LogComplaintCreatedInDb(ILogger<ComplaintCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2506,
            Level = LogLevel.Information,
            Message = "Жалоба с ID: {id} успешно изменена в БД")]
        private static partial void LogComplaintUpdatedInDb(ILogger<ComplaintCommandService> logger, long id);

        [LoggerMessage(
            EventId = 2507,
            Level = LogLevel.Information,
            Message = "Статус жалобы с ID: {id} успешно изменен в БД на {newStatus}")]
        private static partial void LogComplaintStatusChangedInDb(ILogger<ComplaintCommandService> logger, long id, ComplaintStatus newStatus);

        [LoggerMessage(
            EventId = 2508,
            Level = LogLevel.Information,
            Message = "Жалоба с ID: {id} успешно удалена из БД")]
        private static partial void LogComplaintDeletedFromDb(ILogger<ComplaintCommandService> logger, long id);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2511,
            Level = LogLevel.Warning,
            Message = "Операция прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<ComplaintCommandService> logger, long id);

        #endregion
    }
}
