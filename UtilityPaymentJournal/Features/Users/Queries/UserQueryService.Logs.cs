namespace UtilityPaymentJournal.Features.Users.Queries
{
    public partial class UserQueryService
    {
        #region Отладочная информация (Уровень Debug)

        [LoggerMessage(
            EventId = 2701,
            Level = LogLevel.Debug,
            Message = "Запрошено получение развернутых данных из БД пользователя по идентификатору: '{userId}'.")]
        private static partial void LogFetchingUserById(ILogger<UserQueryService> logger, string userId);

        [LoggerMessage(
            EventId = 2703,
            Level = LogLevel.Debug,
            Message = "Запрошена выгрузка полного списка пользователей из БД.")]
        private static partial void LogFetchingAllUsers(ILogger<UserQueryService> logger);

        #endregion

        #region Успешные операции (Уровень Information)

        [LoggerMessage(
            EventId = 2705,
            Level = LogLevel.Information,
            Message = "Данные пользователя с ID '{userId}' успешно извлечены из БД и смапплены в DTO. Назначенная роль: '{roleName}'.")]
        private static partial void LogUserSuccessfullyFetchedFromDb(ILogger<UserQueryService> logger, string userId, string roleName);

        [LoggerMessage(
            EventId = 2706,
            Level = LogLevel.Information,
            Message = "Список пользователей успешно выгружен из БД и преобразован в коллекцию DTO. Всего записей: {count}.")]
        private static partial void LogAllUsersSuccessfullyFetchedFromDb(ILogger<UserQueryService> logger, int count);

        #endregion

        #region Предупреждения и Ошибки (Уровень Warning / Error)

        [LoggerMessage(
            EventId = 2707,
            Level = LogLevel.Warning,
            Message = "Запрошенная учетная запись пользователя с идентификатором '{userId}' отсутствует в БД.")]
        private static partial void LogUserNotFoundInDb(ILogger<UserQueryService> logger, string userId);

        #endregion
    }
}
