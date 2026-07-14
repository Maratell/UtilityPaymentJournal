using Serilog.Context;
using System.Security.Claims;
using UtilityPaymentJournal.Common.Constants;

namespace UtilityPaymentJournal.Infrastructure.Middlewares
{
    /// <summary>
    /// Промежуточное ПО (Middleware) для автоматического обогащения контекста логирования.
    /// Извлекает уникальный идентификатор пользователя (ID) из текущей сессии Identity 
    /// и добавляет его в область видимости логгера (Logging Scope) для всех последующих
    /// логов в рамках текущего HTTP-запроса.
    /// </summary>
    public sealed class UserLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Находим Claim, содержащий ID пользователя (в Identity это NameIdentifier)
            string? userId = context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                // Создаем структурированную область видимости. 
                // Ключ "UserId" автоматически добавится во все логи (даже внутри сервисов),
                // которые будут вызваны внутри этого асинхронного контекста.
                using (LogContext.PushProperty(LogPropertyConstants.UserId, userId))
                {
                    await next(context);
                }
            }
            else
            {
                // Если пользователь анонимный, просто передаем управление дальше по конвейеру
                await next(context);
            }
        }
    }
}
