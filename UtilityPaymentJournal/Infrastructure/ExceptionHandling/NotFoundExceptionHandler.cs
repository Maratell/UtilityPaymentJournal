using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Глобальный обработчик исключений отсутствия ресурсов (KeyNotFoundException).
    /// Реализует стандарт обработки ошибок <see cref="IExceptionHandler"/> (.NET 8+).
    /// </summary>
    /// <remarks>
    /// Перехватывает бизнес-ошибки поиска элементов по ID 
    /// и преобразует их в формат RFC 7807 (Problem Details for HTTP APIs).
    /// </remarks>
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public NotFoundExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not KeyNotFoundException keyNotFoundException)
            {
                return false; // Ошибка не нашего профиля, передаем следующему обработчику
            }

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            // Формируем стандартизированный ответ через встроенный сервис
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Ресурс не найден",
                    Detail = keyNotFoundException.Message 
                }
            });
        }
    }
}
