using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            // Ищем PostgresException на любом уровне вложенности
            var currentEx = exception;
            PostgresException? postgresException = null;

            while (currentEx != null)
            {
                if (currentEx is PostgresException pgEx)
                {
                    postgresException = pgEx;
                    break;
                }
                currentEx = currentEx.InnerException;
            }

            // Если нашли и это нарушение внешнего ключа
            if (postgresException != null && postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

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

        //public async ValueTask<bool> TryHandleAsync(
        //    HttpContext httpContext,
        //    Exception exception,
        //    CancellationToken cancellationToken)
        //{
        //    // Распаковываем исключение: ищем PostgresException на самом верхнем уровне 
        //    // или внутри InnerException (это сработает и для ExecuteDeleteAsync, и для SaveChangesAsync)
        //    if (exception is PostgresException postgresException ||
        //       (exception.InnerException is PostgresException innerPgException && (postgresException = innerPgException) != null))
        //    {
        //        if (postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        //        {
        //            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        //            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        //            {
        //                HttpContext = httpContext,
        //                Exception = exception,
        //                ProblemDetails = new ProblemDetails
        //                {
        //                    Status = StatusCodes.Status409Conflict,
        //                    Title = "Удаление невозможно",
        //                    Detail = "Этот объект связан с другими данными в системе. Сначала удалите связанные элементы."
        //                }
        //            });
        //        }
        //    }

        //    return false;
        //}

        //public async ValueTask<bool> TryHandleAsync(
        //    HttpContext httpContext,
        //    Exception exception,
        //    CancellationToken cancellationToken)
        //{
        //    // Современный паттерн-матчинг: проверяем всю цепочку исключений в одну строчку
        //    if (exception is DbUpdateException { InnerException: PostgresException postgresException }
        //        && postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        //    {
        //        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        //        // Используем стандартный TryWriteAsync, который сам сформирует JSON
        //        // и автоматически добавит важный для отладки "traceId"
        //        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        //        {
        //            HttpContext = httpContext,
        //            Exception = exception,
        //            ProblemDetails = new ProblemDetails
        //            {
        //                Status = StatusCodes.Status409Conflict,
        //                Title = "Удаление невозможно",
        //                Detail = "Этот объект связан с другими данными в системе. Сначала удалите связанные элементы."
        //            }
        //        });
        //    }

        //    return false;
        //}
    }
}
