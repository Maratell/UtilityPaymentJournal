namespace UtilityPaymentJournal.Features.Users.Commands
{
    public partial class UserCommandService
    {
        #region Отладочная информация (Уровень Debug)

        [LoggerMessage(
            EventId = 2750,
            Level = LogLevel.Debug,
            Message = "Запущена процедура транзакционного создания пользователя '{userName}'.")]
        private static partial void LogUserCreationRequested(ILogger<UserCommandService> logger, string userName);

        [LoggerMessage(
            EventId = 2751,
            Level = LogLevel.Debug,
            Message = "Выполняется UserManager.CreateAsync для сохранения записи пользователя '{userName}'.")]
        private static partial void LogExecutingIdentityUserCreate(ILogger<UserCommandService> logger, string userName);

        [LoggerMessage(
            EventId = 2752,
            Level = LogLevel.Debug,
            Message = "Проверка существования системной роли '{roleName}' в таблице AspNetRoles.")]
        private static partial void LogCheckingRoleExistence(ILogger<UserCommandService> logger, string roleName);

        [LoggerMessage(
            EventId = 2753,
            Level = LogLevel.Debug,
            Message = "Роль '{roleName}' отсутствует в системе. Выполняется RoleManager.CreateAsync.")]
        private static partial void LogExecutingIdentityRoleCreate(ILogger<UserCommandService> logger, string roleName);

        [LoggerMessage(
            EventId = 2754,
            Level = LogLevel.Debug,
            Message = "Связывание учетной записи '{userName}' с системной ролью '{roleName}'.")]
        private static partial void LogAssigningRoleToUser(ILogger<UserCommandService> logger, string userName, string roleName);

        [LoggerMessage(
            EventId = 2755,
            Level = LogLevel.Debug,
            Message = "Запущена процедура удаления пользователя с идентификатором: '{userId}'.")]
        private static partial void LogUserDeletionRequested(ILogger<UserCommandService> logger, string userId);

        #endregion

        #region Успешные операции (Уровень Information)

        [LoggerMessage(
            EventId = 2756,
            Level = LogLevel.Information,
            Message = "Транзакция успешно подтверждена. Пользователь '{userName}' создан в базе данных.")]
        private static partial void LogUserCreationTransactionCommitted(ILogger<UserCommandService> logger, string userName);

        [LoggerMessage(
            EventId = 2757,
            Level = LogLevel.Information,
            Message = "Учетная запись с ID '{userId}' (Логин: '{userName}') успешно удалена из системы через DeleteAsync.")]
        private static partial void LogUserSuccessfullyDeleted(ILogger<UserCommandService> logger, string userId, string userName);

        #endregion

        #region Предупреждения и Ошибки (Уровень Warning / Error)

        [LoggerMessage(
            EventId = 2758,
            Level = LogLevel.Warning,
            Message = "Ошибка валидации Identity: Не удалось создать запись пользователя '{userName}' в БД.")]
        private static partial void LogIdentityUserCreateFailed(ILogger<UserCommandService> logger, string userName);

        [LoggerMessage(
            EventId = 2759,
            Level = LogLevel.Error,
            Message = "Критическая ошибка роли: Не удалось сгенерировать новую системную роль '{roleName}'.")]
        private static partial void LogIdentityRoleCreateFailed(ILogger<UserCommandService> logger, string roleName);

        [LoggerMessage(
            EventId = 2760,
            Level = LogLevel.Warning,
            Message = "Ошибка привязки: Не удалось назначить роль '{roleName}' пользователю '{userName}'.")]
        private static partial void LogRoleAssignmentFailed(ILogger<UserCommandService> logger, string userName, string roleName);

        [LoggerMessage(
            EventId = 2761,
            Level = LogLevel.Warning,
            Message = "Сбой транзакции создания пользователя '{userName}'. Выполняется принудительный откат изменений RollbackAsync.")]
        private static partial void LogRollingBackUserCreationTransaction(ILogger<UserCommandService> logger, string userName);

        [LoggerMessage(
            EventId = 2762,
            Level = LogLevel.Warning,
            Message = "Отказ операции: Пользователь для удаления с идентификатором '{userId}' не найден в системе.")]
        private static partial void LogUserToDeleteNotFound(ILogger<UserCommandService> logger, string userId);

        [LoggerMessage(
            EventId = 2763,
            Level = LogLevel.Warning,
            Message = "Ошибка удаления Identity: Не удалось удалить запись пользователя с ID '{userId}'.")]
        private static partial void LogUserDeletionFailed(ILogger<UserCommandService> logger, string userId);

        #endregion
    }
}
