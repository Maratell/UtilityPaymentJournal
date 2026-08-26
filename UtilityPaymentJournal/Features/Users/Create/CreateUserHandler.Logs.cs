
namespace UtilityPaymentJournal.Features.Users.Create
{
    public partial class CreateUserHandler
    {
        [LoggerMessage(
            EventId = 2750,
            Level = LogLevel.Debug,
            Message = "Запущена процедура транзакционного создания пользователя '{userName}'.")]
        private static partial void LogUserCreationRequested(ILogger<CreateUserHandler> logger, string userName);

        [LoggerMessage(
            EventId = 2752,
            Level = LogLevel.Debug,
            Message = "Проверка существования системной роли '{roleName}' в таблице AspNetRoles.")]
        private static partial void LogCheckingRoleExistence(ILogger<CreateUserHandler> logger, string roleName);

        [LoggerMessage(
            EventId = 2759,
            Level = LogLevel.Error,
            Message = "Критическая ошибка роли: Не удалось сгенерировать новую системную роль '{roleName}'.")]
        private static partial void LogIdentityRoleCreateFailed(ILogger<CreateUserHandler> logger, string roleName);

        [LoggerMessage(
            EventId = 2754,
            Level = LogLevel.Debug,
            Message = "Связывание учетной записи '{userName}' с системной ролью '{roleName}'.")]
        private static partial void LogAssigningRoleToUser(ILogger<CreateUserHandler> logger, string userName, string roleName);

        [LoggerMessage(
            EventId = 2760,
            Level = LogLevel.Warning,
            Message = "Ошибка привязки: Не удалось назначить роль '{roleName}' пользователю '{userName}'.")]
        private static partial void LogRoleAssignmentFailed(ILogger<CreateUserHandler> logger, string userName, string roleName);

        [LoggerMessage(
            EventId = 2756,
            Level = LogLevel.Information,
            Message = "Транзакция успешно подтверждена. Пользователь '{userName}' создан в базе данных.")]
        private static partial void LogUserCreationTransactionCommitted(ILogger<CreateUserHandler> logger, string userName);

        [LoggerMessage(
            EventId = 2761,
            Level = LogLevel.Warning,
            Message = "Сбой транзакции создания пользователя '{userName}'. Выполняется принудительный откат изменений RollbackAsync.")]
        private static partial void LogRollingBackUserCreationTransaction(ILogger<CreateUserHandler> logger, string userName);
    }
}
