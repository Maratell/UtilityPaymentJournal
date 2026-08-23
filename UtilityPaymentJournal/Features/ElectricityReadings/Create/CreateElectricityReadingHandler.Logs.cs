
namespace UtilityPaymentJournal.Features.ElectricityReadings.Create
{
    public partial class CreateElectricityReadingHandler
    {
        [LoggerMessage(
            EventId = 2401,
            Level = LogLevel.Information,
            Message = "Запрос на сохранение показания счетчика электроэнергии в БД. Значение: {currentValue}")]
        private static partial void LogElectricityReadingCreationRequested(ILogger<CreateElectricityReadingHandler> logger, long currentValue);

        [LoggerMessage(
            EventId = 2404,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии успешно сохранено в БД. Записи присвоен ID: {id}")]
        private static partial void LogElectricityReadingCreatedInDb(ILogger<CreateElectricityReadingHandler> logger, long id);
    }
}
