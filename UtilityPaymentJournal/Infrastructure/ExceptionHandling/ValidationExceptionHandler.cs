using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Перехватчик ошибок валидации FluentValidation.
    /// Преобразует исключение в ответ 400 Bad Request со списком конкретных нарушений.
    /// </summary>
    public class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        private readonly ILogger<ValidationExceptionHandler> _logger;

        public ValidationExceptionHandler(
            IProblemDetailsService problemDetailsService,
            ILogger<ValidationExceptionHandler> logger)
        {
            _problemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Если это НЕ ошибка валидации, возвращаем false и передаем ход следующему обработчику в цепочке
            if (exception is not ValidationException validationException)
            {
                return false;
            }

            _logger.LogWarning("Запрос не прошел валидацию бизнес-правил: {Message}", exception.Message);

            // Группируем ошибки по полям: "Address" -> ["Длина от 5 символов", "Поле обязательно"]
            var validationErrors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка валидации параметров запроса",
                Detail = "Один или несколько параметров запроса не прошли проверку.",
                Instance = httpContext.Request.Path
            };

            // Добавляем словарь с детализацией ошибок по полям в расширения ProblemDetails
            problemDetails.Extensions.Add("errors", validationErrors);

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }
    }
}
