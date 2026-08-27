using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace UtilityPaymentJournal.Tests.Integration.Infrastructure
{
    /// <summary>
    /// Тестовая заглушка (Mock/Fake) для встроенной системы защиты от CSRF/XSRF-атак (Antiforgery).
    /// </summary>
    /// <remarks>
    /// В интеграционных тестах реальная валидация антиподделочных токенов является избыточной, 
    /// так как виртуальный HttpClient не хранит сессионное состояние браузера по умолчанию. 
    /// Данный класс принудительно одобряет любой входящий запрос, предотвращая падение тестов 
    /// с ошибкой "400 Bad Request" на эндпоинтах, защищенных точечным атрибутом [ValidateAntiForgeryToken].
    /// </remarks>
    public class StubAntiforgery : IAntiforgery
    {
        /// <summary>
        /// Имитирует генерацию и сохранение токенов безопасности для текущего HTTP-контекста.
        /// Возвращает предопределенный набор фиктивных строковых значений-заглушек.
        /// </summary>
        public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
            => new("fake-token", "fake-token", "fake-form", "fake-header");

        /// <summary>
        /// Имитирует чтение существующих токенов безопасности из текущего HTTP-контекста.
        /// Возвращает предопределенный набор фиктивных строковых значений-заглушек.
        /// </summary>
        public AntiforgeryTokenSet GetTokens(HttpContext httpContext)
            => new("fake-token", "fake-token", "fake-form", "fake-header");

        /// <summary>
        /// Асинхронно проверяет валидность токена антиподделки в контексте текущего запроса.
        /// </summary>
        /// <returns>Всегда возвращает <see langword="true"/>, гарантируя успешное прохождение проверки безопасности.</returns>
        public Task<bool> IsRequestValidAsync(HttpContext httpContext)
            => Task.FromResult(true);

        /// <summary>
        /// Выполняет синхронную валидацию токенов антиподделки для текущего HTTP-запроса.
        /// </summary>
        /// <remarks>
        /// Тело метода намеренно оставлено пустым. Отсутствие генерации исключения AntiforgeryValidationException
        /// расценивается конвейером ASP.NET Core как успешное прохождение проверки.
        /// </remarks>
        public void ValidateRequest(HttpContext httpContext)
        {
            // Метод ничего не делает, имитируя успешное прохождение валидации безопасности
        }

        /// <summary>
        /// Выполняет асинхронную валидацию токенов антиподделки для текущего HTTP-запроса.
        /// </summary>
        /// <returns>Возвращает успешно завершенную задачу <see cref="Task.CompletedTask"/>, сообщая пайплайну о валидности запроса.</returns>
        public Task ValidateRequestAsync(HttpContext httpContext)
            => Task.CompletedTask;

        /// <summary>
        /// Имитирует установку токена безопасности в Cookie браузера и в заголовки HTTP-ответа.
        /// </summary>
        /// <remarks>
        /// Тело метода оставлено пустым, чтобы избавить интеграционные тесты от необходимости 
        /// обрабатывать реальные Set-Cookie заголовки в виртуальном HttpClient.
        /// </remarks>
        public void SetCookieTokenAndHeader(HttpContext httpContext)
        {
            // Метод ничего не делает, избавляя тесты от генерации реальных кук
        }
    }
}
