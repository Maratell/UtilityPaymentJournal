using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Controllers.Api
{
    public partial class ResidencesApiController
    {
        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 1008, Level = LogLevel.Information, Message = "Запрос на создание жилого объекта. Адрес: {address}")]
        private static partial void LogResidenceCreationRequested(ILogger<ResidencesApiController> logger, string address);

        [LoggerMessage(EventId = 1009, Level = LogLevel.Information, Message = "Запрос на обновление жилого объекта {id}. Новый адрес: {address}")]
        private static partial void LogResidenceUpdateRequested(ILogger<ResidencesApiController> logger, long id, string address);

        [LoggerMessage(EventId = 1010, Level = LogLevel.Information, Message = "Запрос на удаление жилого объекта {id}")]
        private static partial void LogResidenceDeletionRequested(ILogger<ResidencesApiController> logger, long id);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Жилой объект {id} удален")]
        private static partial void LogResidenceDeleted(ILogger<ResidencesApiController> logger, long id);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Создан жилой объект {id}")]
        private static partial void LogResidenceCreated(ILogger<ResidencesApiController> logger, long id);

        [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Обновлен жилой объект {id}")]
        private static partial void LogResidenceUpdated(ILogger<ResidencesApiController> logger, long id);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1004, Level = LogLevel.Debug, Message = "Запрос на получение всех жилых объектов")]
        private static partial void LogFetchingAllResidences(ILogger<ResidencesApiController> logger);

        [LoggerMessage(EventId = 1005, Level = LogLevel.Debug, Message = "Получено {count} жилых объектов")]
        private static partial void LogFetchedAllResidencesCount(ILogger<ResidencesApiController> logger, int count);

        #endregion
    }
}
