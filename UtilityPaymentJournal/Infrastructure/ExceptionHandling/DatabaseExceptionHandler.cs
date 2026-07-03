using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace UtilityPaymentJournal.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Глобальный обработчик исключений уровня базы данных (PostgreSQL / EF Core).
    /// Реализует стандарт обработки ошибок <see cref="IExceptionHandler"/> (.NET 8+).
    /// </summary>
    /// <remarks>
    /// Перехватывает ошибки обновления данных, распознает специфичные коды СУБД 
    /// с помощью типизированного драйвера <see cref="PostgresException"/> 
    /// и преобразует их в формат RFC 7807 (Problem Details for HTTP APIs).
    /// </remarks>
    public class DatabaseExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        // Внедряем встроенный системный сервис для генерации ProblemDetails
        public DatabaseExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Современный паттерн-матчинг: проверяем всю цепочку исключений в одну строчку
            if (exception is DbUpdateException { InnerException: PostgresException postgresException }
                && postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

                // Используем стандартный TryWriteAsync, который сам сформирует JSON
                // и автоматически добавит важный для отладки "traceId"
                return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    Exception = exception,
                    ProblemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Удаление невозможно",
                        Detail = "Этот объект связан с другими данными в системе. Сначала удалите связанные элементы."
                    }
                });
            }

            return false;
        }
    }
}
