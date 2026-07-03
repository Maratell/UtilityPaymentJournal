using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Финальный обработчик всех необработанных и непредвиденных исключений приложения.
    /// Реализует стандарт обработки ошибок <see cref="IExceptionHandler"/> (.NET 8+).
    /// </summary>
    /// <remarks>
    /// Перехватывает любые сбои верхнего уровня, логирует их через системный ILogger 
    /// и преобразует в безопасный формат RFC 7807 (Problem Details for HTTP APIs), 
    /// скрывая stack trace на продакшене.
    /// </remarks>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        // Внедряем логгер и параметры среды разработки
        public GlobalExceptionHandler(
            IProblemDetailsService problemDetailsService,
            ILogger<GlobalExceptionHandler> logger,
            IHostEnvironment env)
        {
            _problemDetailsService = problemDetailsService;
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Логируем критическую ошибку в консоль/файл
            _logger.LogError(exception, "Произошло необработанное исключение: {Message}", exception.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Для режима разработки (Development) выводим полный стек ошибки,
            // а для продакшена (Production) — скрываем детали из соображений безопасности.
            string errorDetail = _env.IsDevelopment()
                ? exception.ToString()
                : "Внутренняя ошибка сервера. Пожалуйста, обратитесь к администратору.";

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Критическая ошибка приложения",
                    Detail = errorDetail
                }
            });
        }
    }
}
