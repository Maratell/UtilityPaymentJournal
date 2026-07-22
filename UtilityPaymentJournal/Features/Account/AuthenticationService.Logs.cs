namespace UtilityPaymentJournal.Features.Account
{
    public partial class AuthenticationService
    {
        #region Успешные операции (Уровень Information)

        [LoggerMessage(
            EventId = 2601,
            Level = LogLevel.Information,
            Message = "Пользователь '{userName}' успешно аутентифицирован в системе.")]
        private static partial void LogUserSignedIn(ILogger<AuthenticationService> logger, string userName);

        [LoggerMessage(
            EventId = 2602,
            Level = LogLevel.Information,
            Message = "Пользователь успешно вышел из системы (сессия завершена).")]
        private static partial void LogUserSignedOut(ILogger<AuthenticationService> logger);

        #endregion

        #region Предупреждения безопасности (Уровень Warning)

        [LoggerMessage(
            EventId = 2603,
            Level = LogLevel.Warning,
            Message = "Неудачная попытка входа. Неверные учетные данные для пользователя: '{userName}'.")]
        private static partial void LogUserSignInFailed(ILogger<AuthenticationService> logger, string userName);

        [LoggerMessage(
            EventId = 2604,
            Level = LogLevel.Warning,
            Message = "Вход заблокирован: учетная запись пользователя '{userName}' временно заблокирована из-за превышения лимита ошибок.")]
        private static partial void LogUserLockedOut(ILogger<AuthenticationService> logger, string userName);

        [LoggerMessage(
            EventId = 2605,
            Level = LogLevel.Warning,
            Message = "Вход отклонен: пользователю '{userName}' запрещен доступ к системе на уровне бизнес-логики.")]
        private static partial void LogUserLoginNotAllowed(ILogger<AuthenticationService> logger, string userName);

        #endregion
    }
}
