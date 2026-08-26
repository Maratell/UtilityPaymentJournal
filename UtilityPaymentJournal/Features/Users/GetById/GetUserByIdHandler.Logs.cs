
namespace UtilityPaymentJournal.Features.Users.GetById
{
    public partial class GetUserByIdHandler
    {
        [LoggerMessage(
            EventId = 2701,
            Level = LogLevel.Debug,
            Message = "Запрошено получение развернутых данных из БД пользователя по идентификатору: '{userId}'.")]
        private static partial void LogFetchingUserById(ILogger<GetUserByIdHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2707,
            Level = LogLevel.Warning,
            Message = "Запрошенная учетная запись пользователя с идентификатором '{userId}' отсутствует в БД.")]
        private static partial void LogUserNotFoundInDb(ILogger<GetUserByIdHandler> logger, string userId);

        [LoggerMessage(
            EventId = 2705,
            Level = LogLevel.Information,
            Message = "Данные пользователя с ID '{userId}' успешно извлечены из БД и смапплены в DTO. Назначенная роль: '{roleName}'.")]
        private static partial void LogUserSuccessfullyFetchedFromDb(ILogger<GetUserByIdHandler> logger, string userId, string roleName);
    }
}
