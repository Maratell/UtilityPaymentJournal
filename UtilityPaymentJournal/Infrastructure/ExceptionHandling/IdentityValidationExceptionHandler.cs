using Microsoft.AspNetCore.Diagnostics;
using UtilityPaymentJournal.Common.Exceptions;

namespace UtilityPaymentJournal.Infrastructure.ExceptionHandling
{
    public class IdentityValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Если это не наше кастомное исключение — пропускаем его дальше по цепочке
            if (exception is not IdentityValidationException validationException)
            {
                return false;
            }

            // Перехватываем, формируем BadRequest (400) и отдаем красивый JSON с ошибками
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                title = "Ошибка валидации данных",
                status = StatusCodes.Status400BadRequest,
                errors = validationException.Errors
            }, cancellationToken);

            return true; // Говорим ASP.NET Core, что ошибка успешно обработана
        }
    }
}
