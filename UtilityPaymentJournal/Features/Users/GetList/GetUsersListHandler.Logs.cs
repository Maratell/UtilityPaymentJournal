
namespace UtilityPaymentJournal.Features.Users.GetList
{
    public partial class GetUsersListHandler
    {
        [LoggerMessage(
            EventId = 2703,
            Level = LogLevel.Debug,
            Message = "Запрошена выгрузка полного списка пользователей из БД.")]
        private static partial void LogFetchingAllUsers(ILogger<GetUsersListHandler> logger);

        [LoggerMessage(
            EventId = 2706,
            Level = LogLevel.Information,
            Message = "Список пользователей успешно выгружен из БД и преобразован в коллекцию DTO. Всего записей: {count}.")]
        private static partial void LogAllUsersSuccessfullyFetchedFromDb(ILogger<GetUsersListHandler> logger, int count);
    }
}
