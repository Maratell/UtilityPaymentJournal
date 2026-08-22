
namespace UtilityPaymentJournal.Features.WaterReadings.Create
{
    public partial class CreateWaterReadingHandler
    {
        [LoggerMessage(
            EventId = 2301,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение показания счетчика воды в БД. Значение: {currentValue}")]
        private static partial void LogWaterReadingCreationRequested(ILogger<CreateWaterReadingHandler> logger, long currentValue);

        [LoggerMessage(
            EventId = 2304,
            Level = LogLevel.Information,
            Message = "Показание счетчика воды успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogWaterReadingCreatedInDb(ILogger<CreateWaterReadingHandler> logger, long id);
    }
}
