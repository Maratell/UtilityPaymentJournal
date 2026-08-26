
namespace UtilityPaymentJournal.Features.Users.Delete
{
    public partial class DeleteUserHandler
    {
        [LoggerMessage(
            EventId = 2755,
            Level = LogLevel.Debug,
            Message = "Запущена процедура удаления пользователя с идентификатором: '{userId}'.")]
        private static partial void LogUserDeletionRequested(ILogger<DeleteUserHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2762,
            Level = LogLevel.Warning,
            Message = "Отказ операции: Пользователь для удаления с идентификатором '{userId}' не найден в системе.")]
        private static partial void LogUserToDeleteNotFound(ILogger<DeleteUserHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2763,
            Level = LogLevel.Warning,
            Message = "Ошибка удаления Identity: Не удалось удалить запись пользователя с ID '{userId}'.")]
        private static partial void LogUserDeletionFailed(ILogger<DeleteUserHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2757,
            Level = LogLevel.Information,
            Message = "Учетная запись с ID '{userId}' (Логин: '{userName}') успешно удалена из системы через DeleteAsync.")]
        private static partial void LogUserSuccessfullyDeleted(ILogger<DeleteUserHandler> logger, string userId, string userName);
    }
}
