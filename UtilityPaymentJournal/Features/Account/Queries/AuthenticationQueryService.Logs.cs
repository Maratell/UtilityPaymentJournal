namespace UtilityPaymentJournal.Features.Account.Queries
{
    public partial class AuthenticationQueryService
    {
        #region Отладочная информация (Уровень Debug)

        [LoggerMessage(
            EventId = 2606,
            Level = LogLevel.Debug,
            Message = "Запущена проверка статуса аутентификации текущего пользователя.")]
        private static partial void LogCheckingAuthenticationStatus(ILogger<AuthenticationQueryService> logger);

        [LoggerMessage(
            EventId = 2607,
            Level = LogLevel.Debug,
            Message = "Статус аутентификации проверен. Результат: Пользователь вошел = {isAuthenticated}.")]
        private static partial void LogAuthenticationStatusChecked(ILogger<AuthenticationQueryService> logger, bool isAuthenticated);

        [LoggerMessage(
            EventId = 2608,
            Level = LogLevel.Debug,
            Message = "Запрошен идентификатор (ID) текущего пользователя из данных сессии.")]
        private static partial void LogFetchingCurrentUserId(ILogger<AuthenticationQueryService> logger);

        [LoggerMessage(
            EventId = 2609,
            Level = LogLevel.Debug,
            Message = "Идентификатор текущего пользователя извлечен. Значение ID: '{userId}'.")]
        private static partial void LogCurrentUserIdFetched(ILogger<AuthenticationQueryService> logger, string? userId);

        [LoggerMessage(
            EventId = 2610,
            Level = LogLevel.Debug,
            Message = "Запрошено получение развернутых данных (Details) текущего пользователя.")]
        private static partial void LogFetchingCurrentUserDetails(ILogger<AuthenticationQueryService> logger);

        [LoggerMessage(
            EventId = 2611,
            Level = LogLevel.Debug,
            Message = "Выполняется запрос к базе данных для поиска пользователя с ID: '{userId}'.")]
        private static partial void LogFetchingUserFromDb(ILogger<AuthenticationQueryService> logger, string userId);

        #endregion

        #region Успешные операции (Уровень Information)

        [LoggerMessage(
            EventId = 2612,
            Level = LogLevel.Information,
            Message = "Данные пользователя с ID '{userId}' успешно извлечены из базы данных и смапплены в DTO.")]
        private static partial void LogUserSuccessfullyFetchedFromDb(ILogger<AuthenticationQueryService> logger, string userId);

        #endregion

        #region Предупреждения и Ошибки (Уровень Warning / Error)

        [LoggerMessage(
            EventId = 2613,
            Level = LogLevel.Warning,
            Message = "Отказ в доступе: Анонимный пользователь попытался запросить детальные данные профиля.")]
        private static partial void LogUnauthorizedDetailsRequest(ILogger<AuthenticationQueryService> logger);

        [LoggerMessage(
            EventId = 2614,
            Level = LogLevel.Error,
            Message = "Критическое несоответствие данных: Сессия активна для ID '{userId}', но учетная запись отсутствует в базе данных.")]
        private static partial void LogUserNotFoundInDb(ILogger<AuthenticationQueryService> logger, string userId);

        #endregion
    }
}
