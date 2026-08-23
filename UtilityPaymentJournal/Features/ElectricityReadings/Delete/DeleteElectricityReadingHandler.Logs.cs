
namespace UtilityPaymentJournal.Features.ElectricityReadings.Delete
{
    public partial class DeleteElectricityReadingHandler
    {
        [LoggerMessage(
            EventId = 2403,
            Level = LogLevel.Information,
            Message = "Запрос на удаление показания счетчика электроэнергии из БД. ID записи: {id}")]
        private static partial void LogElectricityReadingDeletionRequested(ILogger<DeleteElectricityReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2411,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<DeleteElectricityReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2406,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии с ID: {id} успешно удалено из БД")]
        private static partial void LogElectricityReadingDeletedFromDb(ILogger<DeleteElectricityReadingHandler> logger, long id);
    }
}
