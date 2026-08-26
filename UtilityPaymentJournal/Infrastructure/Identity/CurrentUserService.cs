using System.Security.Claims;

namespace UtilityPaymentJournal.Infrastructure.Identity
{
    /// <summary>
    /// Инфраструктурный сервис для предоставления данных о текущем контексте пользователя.
    /// Обеспечивает потокобезопасный доступ к метаданным активной сессии через системный аксессор HTTP-контекста.
    /// </summary>
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        /// <summary>
        /// Уникальный строковый идентификатор (ID) пользователя в системе Identity, зафиксированный в текущей сессии.
        /// Возвращает null, если запрос выполняется анонимным гостем.
        /// </summary>
        public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
