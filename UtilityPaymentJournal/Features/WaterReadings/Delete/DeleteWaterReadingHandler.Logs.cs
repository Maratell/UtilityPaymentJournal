
namespace UtilityPaymentJournal.Features.WaterReadings.Delete
{
    public partial class DeleteWaterReadingHandler
    {
        [LoggerMessage(
            EventId = 2303,
            Level = LogLevel.Information,
            Message = "Запрос на удаление показания счетчика воды из БД. ID записи: {id}")]
        private static partial void LogWaterReadingDeletionRequested(ILogger<DeleteWaterReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2311,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<DeleteWaterReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2306,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды с ID: {id} успешно удалено из БД")]
        private static partial void LogWaterReadingDeletedFromDb(ILogger<DeleteWaterReadingHandler> logger, long id);
    }
}
