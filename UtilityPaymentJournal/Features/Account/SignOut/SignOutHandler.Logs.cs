
namespace UtilityPaymentJournal.Features.Account.SignOut
{
    public partial class SignOutHandler
    {
        [LoggerMessage(
            EventId = 2602,
            Level = LogLevel.Information,
            Message = "Пользователь успешно вышел из системы (сессия завершена).")]
        private static partial void LogUserSignedOut(ILogger<SignOutHandler> logger);
    }
}
