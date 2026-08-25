
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.ChangeStatus
{
    public partial class ChangeComplaintStatusHandler
    {
        [LoggerMessage(
            EventId = 2503,
            Level = LogLevel.Information,
            Message = "Запрос на точечное изменение статуса жалобы в БД. ID записи: {id}. Целевой статус: {newStatus}")]
        private static partial void LogComplaintStatusChangeRequested(ILogger<ChangeComplaintStatusHandler> logger, long id, ComplaintStatus newStatus);

        [LoggerMessage(
            EventId = 2511,
            Level = LogLevel.Warning,
            Message = "Операция прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<ChangeComplaintStatusHandler> logger, long id);

        [LoggerMessage(
            EventId = 2507,
            Level = LogLevel.Information,
            Message = "Статус жалобы с ID: {id} успешно изменен в БД на {newStatus}")]
        private static partial void LogComplaintStatusChangedInDb(ILogger<ChangeComplaintStatusHandler> logger, long id, ComplaintStatus newStatus);
    }
}
