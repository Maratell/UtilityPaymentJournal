namespace UtilityPaymentJournal.Features.Account.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных об аутентификации и текущей сессии.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния сессий или БД в рамках паттерна CQRS.
    /// </summary>
    public interface IAuthenticationQueryService
    {
        /// <summary>
        /// Проверить, является ли пользователь текущего HTTP-контекста аутентифицированным в системе.
        /// </summary>
        /// <returns>Значение true, если сессия пользователя активна и подтверждена; иначе — false</returns>
        bool IsAuthenticated();
        /// <summary>
        /// Получить уникальный идентификатор текущего аутентифицированного пользователя.
        /// </summary>
        /// <returns>Строковый идентификатор (ID) пользователя из заклеймленных данных сессии или null, если пользователь не вошел в систему</returns>
        string? GetCurrentUserId();
        /// <summary>
        /// Получить развернутые учетные данные текущего пользователя из базы данных на основе его активной сессии.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с детальными данными пользователя для профиля или интерфейса</returns>
        /// <exception cref="UnauthorizedAccessException">Выбрасывается, если запрос выполнен неавторизованным пользователем</exception>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если идентификатор пользователя отсутствует в сессии или его учетная запись не найдена в базе данных</exception>
        Task<CurrentUserQueryResultDto> GetCurrentUserDetailsAsync(CancellationToken cancellationToken = default);
    }
}
