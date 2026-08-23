
namespace UtilityPaymentJournal.Features.ElectricityReadings.Edit
{
    public partial class EditElectricityReadingHandler
    {
        [LoggerMessage(
            EventId = 2402,
            Level = LogLevel.Information,
            Message = "Запрос на обновление показания счетчика электроэнергии в БД. ID записи: {id}. Новое значение: {currentValue}")]
        private static partial void LogElectricityReadingUpdateRequested(ILogger<EditElectricityReadingHandler> logger, long id, long currentValue);

        [LoggerMessage(
            EventId = 2411,
            Level = LogLevel.Warning,
            Message = "Операция изменения прервана: показание счетчика электроэнергии с ID: {id} отсутствует в БД")]
        private static partial void LogElectricityReadingNotFoundInDb(ILogger<EditElectricityReadingHandler> logger, long id);

        [LoggerMessage(
            EventId = 2405,
            Level = LogLevel.Information,
            Message = "Показание счетчика электроэнергии с ID: {id} успешно изменено в БД")]
        private static partial void LogElectricityReadingUpdatedInDb(ILogger<EditElectricityReadingHandler> logger, long id);


    }
}
