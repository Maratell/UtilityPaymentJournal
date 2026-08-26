
namespace UtilityPaymentJournal.Features.Account.GetCurrentUser
{
    public partial class GetCurrentUserHandler
    {
        [LoggerMessage(
            EventId = 2610,
            Level = LogLevel.Debug,
            Message = "Запрошено получение развернутых данных (Details) текущего пользователя.")]
        private static partial void LogFetchingCurrentUserDetails(ILogger<GetCurrentUserHandler> logger);

        [LoggerMessage(
           EventId = 2613,
           Level = LogLevel.Warning,
           Message = "Отказ в доступе: Анонимный пользователь попытался запросить детальные данные профиля.")]
        private static partial void LogUnauthorizedDetailsRequest(ILogger<GetCurrentUserHandler> logger);

        [LoggerMessage(
            EventId = 2611,
            Level = LogLevel.Debug,
            Message = "Выполняется запрос к базе данных для поиска пользователя с ID: '{userId}'.")]
        private static partial void LogFetchingUserFromDb(ILogger<GetCurrentUserHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2614,
            Level = LogLevel.Error,
            Message = "Критическое несоответствие данных: Сессия активна для ID '{userId}', но учетная запись отсутствует в базе данных.")]
        private static partial void LogUserNotFoundInDb(ILogger<GetCurrentUserHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2612,
            Level = LogLevel.Information,
            Message = "Данные пользователя с ID '{userId}' успешно извлечены из базы данных и смапплены в DTO.")]
        private static partial void LogUserSuccessfullyFetchedFromDb(ILogger<GetCurrentUserHandler> logger, string userId);
    }
}
