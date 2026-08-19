using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace UtilityPaymentJournal.Tests.Integration
{
    /// <summary>
    /// Тестовый обработчик аутентификации.
    /// Автоматически подписывает каждый входящий HTTP-запрос в тестах фейковыми правами администратора,
    /// позволяя обходить реальные проверки паролей, JWT-токенов или Cookie.
    /// </summary>
    public class TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        /// <summary>
        /// Метод перехватывает проверку авторизации для каждого запроса, отправленного через тестовый HttpClient.
        /// </summary>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 1. Формируем список "паспортных данных" (Claims) для нашего виртуального пользователя.
            // Вы можете добавлять сюда любые данные, которые ваш контроллер ожидает увидеть у пользователя.
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "TestAdmin"),
            new Claim(ClaimTypes.NameIdentifier, "test-admin-id-123"), // Понадобится, если контроллер берет ID текущего юзера
            new Claim(ClaimTypes.Role, "Admin")                        // Позволяет проходить через фильтры [Authorize(Roles = "Admin")]
        };

            // 2. Создаем личность пользователя (Identity) и привязываем её к нашей константе схемы тестов.
            // Использование константы гарантирует, что система авторизации ASP.NET Core правильно сопоставит этот Handler.
            var identity = new ClaimsIdentity(claims, IntegrationTestWebAppFactory.AuthenticationScheme);

            // 3. Создаем субъект (Principal), который представляет собой учетную запись пользователя в контексте запроса.
            var principal = new ClaimsPrincipal(identity);

            // 4. Упаковываем пользователя в "билет аутентификации" (Ticket).
            var ticket = new AuthenticationTicket(principal, IntegrationTestWebAppFactory.AuthenticationScheme);

            // 5. Возвращаем успешный результат. Теперь для ASP.NET Core этот запрос официально считается авторизованным.
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    //public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    //{
    //    public TestAuthHandler(
    //        IOptionsMonitor<AuthenticationSchemeOptions> options,
    //        ILoggerFactory logger,
    //        UrlEncoder encoder) : base(options, logger, encoder) { }

    //    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    //    {
    //        var claims = new[]
    //        {
    //            new Claim(ClaimTypes.Name, "TestAdmin"),
    //            new Claim(ClaimTypes.NameIdentifier, "test-admin-id-123"),
    //            new Claim(ClaimTypes.Role, "Admin")
    //        };

    //        // ВАЖНО: Меняем имя схемы в конструкторах ClaimsIdentity и AuthenticationTicket на "TestAuth"
    //        var identity = new ClaimsIdentity(claims, "TestAuth");
    //        var principal = new ClaimsPrincipal(identity);
    //        var ticket = new AuthenticationTicket(principal, "TestAuth");

    //        return Task.FromResult(AuthenticateResult.Success(ticket));
    //    }
    //}
}
