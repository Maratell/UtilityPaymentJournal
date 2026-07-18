namespace UtilityPaymentJournal.Features.Complaints.Queries
{
    /// <summary>
    /// Partial-класс логов для сервиса запросов жалоб.
    /// Содержит высокопроизводительные методы логирования операций чтения.
    /// </summary>
    public partial class ComplaintQueryService
    {
        #region Чтение данных (Уровень Debug)

        [LoggerMessage(
            EventId = 2521,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение списка всех жалоб")]
        private static partial void LogFetchingAllComplaintsFromDb(ILogger<ComplaintQueryService> logger);

        [LoggerMessage(
            EventId = 2522,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечено {count} записей жалоб")]
        private static partial void LogFetchedAllComplaintsFromDbCount(ILogger<ComplaintQueryService> logger, int count);

        [LoggerMessage(
            EventId = 2523,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение жалобы по ID: {id}")]
        private static partial void LogFetchingComplaintByIdFromDb(ILogger<ComplaintQueryService> logger, long id);

        [LoggerMessage(
            EventId = 2524,
            Level = LogLevel.Debug,
            Message = "Запрос к БД на получение сгруппированных данных для доски жалоб")]
        private static partial void LogFetchingComplaintBoardFromDb(ILogger<ComplaintQueryService> logger);

        [LoggerMessage(
            EventId = 2525,
            Level = LogLevel.Debug,
            Message = "Из БД успешно извлечены и распределены данные для доски жалоб. Всего записей: {count}")]
        private static partial void LogFetchedComplaintBoardFromDb(ILogger<ComplaintQueryService> logger, int count);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(
            EventId = 2531,
            Level = LogLevel.Warning,
            Message = "Операция чтения прервана: жалоба с ID: {id} отсутствует в БД")]
        private static partial void LogComplaintNotFoundInDb(ILogger<ComplaintQueryService> logger, long id);

        #endregion
    }
}
