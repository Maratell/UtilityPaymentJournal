namespace UtilityPaymentJournal.Features.Residences.Queries
{
    public partial class ResidenceQueryService
    {
        #region Процесс получения данных (Уровень Information)

        [LoggerMessage(
            EventId = 2021,
            Level = LogLevel.Information,
            Message = "Запрос на получение данных объекта недвижимости из БД. ID записи: {id}")]
        private static partial void LogFetchingResidenceByIdFromDb(ILogger<ResidenceQueryService> logger, long id);

        [LoggerMessage(
            EventId = 2022,
            Level = LogLevel.Information,
            Message = "Запрос на получение списка всех объектов недвижимости из БД")]
        private static partial void LogFetchingAllResidencesFromDb(ILogger<ResidenceQueryService> logger);

        [LoggerMessage(
            EventId = 2023,
            Level = LogLevel.Information,
            Message = "Успешно получено объектов недвижимости из БД. Количество: {count}")]
        private static partial void LogFetchedAllResidencesFromDbCount(ILogger<ResidenceQueryService> logger, int count);

        #endregion

        #region Ошибки извлечения данных (Уровень Warning)

        [LoggerMessage(
            EventId = 2031,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: объект недвижимости с ID: {id} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger<ResidenceQueryService> logger, long id);

        #endregion
    }
}
