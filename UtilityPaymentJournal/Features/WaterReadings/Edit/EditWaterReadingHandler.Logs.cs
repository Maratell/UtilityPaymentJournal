
namespace UtilityPaymentJournal.Features.WaterReadings.Edit
{
    public partial class EditWaterReadingHandler
    {
        [LoggerMessage(
            EventId = 2302,
            Level = LogLevel.Information,
            Message = "Запрос на обновление показания счетчика воды в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogWaterReadingUpdateRequested(ILogger<EditWaterReadingHandler> logger, long id, long currentValue);

        [LoggerMessage(
            EventId = 2311,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика воды с ID: {id} отсутствует в БД")]
        private static partial void LogWaterReadingNotFoundInDb(ILogger<EditWaterReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2305,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды с ID: {id} успешно изменено в БД")]
        private static partial void LogWaterReadingUpdatedInDb(ILogger<EditWaterReadingHandler> logger, long id);
    }
}
