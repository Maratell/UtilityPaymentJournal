namespace UtilityPaymentJournal.Features.Account.Queries
{
    /// <summary>
    /// ДТО результата запроса детальных данных текущего аутентифицированного пользователя.
    /// Используется для передачи информации о профиле клиенту в UI (GetCurrentUserDetails).
    /// </summary>
    /// <param name="Id">Уникальный строковый идентификатор пользователя в системе Identity</param>
    /// <param name="UserName">Уникальное имя пользователя (логин) для отображения в интерфейсе</param>
    public record CurrentUserQueryResultDto(
        string Id,
        string UserName
    );
}
