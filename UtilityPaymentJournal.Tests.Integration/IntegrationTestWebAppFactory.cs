using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using Respawn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Tests.Integration
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<ApplicationDbContext>, IAsyncLifetime
    {
        // ГЛОБАЛЬНЫЕ СТАТИЧЕСКИЕ РЕСУРСЫ — общие для всей сессии тестов
        private static readonly PostgreSqlContainer _dbContainer;
        private static readonly DbConnection _dbConnection;
        private static readonly Respawner _respawner;
        private static readonly string _connectionString;

        public HttpClient HttpClient { get; private set; } = default!;

        // СТАТИЧЕСКИЙ КОНСТРУКТОР — Вызывается гарантированно ОДИН РАЗ за весь процесс и строго в один поток
        static IntegrationTestWebAppFactory()
        {
            // 1. Настраиваем и синхронно запускаем контейнер
            _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
                .WithDatabase("utility_payment_journal_integration_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            _dbContainer.StartAsync().GetAwaiter().GetResult();
            _connectionString = _dbContainer.GetConnectionString();

            // 2. Изолированно накатываем миграции ДО старта какого-либо веб-сервера
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);

            using (var dbContext = new ApplicationDbContext(optionsBuilder.Options, null!))
            {
                dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            }

            // 3. Настраиваем Respawn для быстрой очистки таблиц public
            _dbConnection = new NpgsqlConnection(_connectionString);
            _dbConnection.OpenAsync().GetAwaiter().GetResult();

            _respawner = Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            }).GetAwaiter().GetResult();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Сообщаем серверу, что среда выполнения интеграционных тестов — IntegrationTesting!
            builder.UseEnvironment("IntegrationTesting");

            builder.ConfigureTestServices(services =>
            {
                // 1. ПОДМЕНА DBCONTEXT С ХАКОМ ПРОТИВ ФОНОВЫХ МИГРАЦИЙ
                var dbDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbDescriptor != null)
                {
                    services.Remove(dbDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_connectionString);
                });

                // 2. ПОДМЕНА АУТЕНТИФИКАЦИИ ДЛЯ ТЕСТОВ (ваш рабочий код)
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestAuth";
                    options.DefaultChallengeScheme = "TestAuth";
                    options.DefaultScheme = "TestAuth";
                    options.DefaultSignInScheme = "TestAuth";
                    options.DefaultSignOutScheme = "TestAuth";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", options => { });

                // 3. ОТКЛЮЧЕНИЕ ГЛОБАЛЬНЫХ ФИЛЬТРОВ RE-DIRECT И CSRF ДЛЯ ТЕСТОВ
                services.PostConfigure<Microsoft.AspNetCore.Authorization.AuthorizationOptions>(options =>
                {
                    options.FallbackPolicy = null; // Отключаем редиректы Identity на /account в тестах
                });

                services.PostConfigure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
                {
                    // Находим и вырезаем глобальный фильтр антиподделки токенов, который генерировал 400 Bad Request
                    var csrfFilter = options.Filters.FirstOrDefault(f =>
                        f is Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute);

                    if (csrfFilter != null)
                    {
                        options.Filters.Remove(csrfFilter);
                    }
                });
            });
        }

        // Вызывается перед каждым тестовым классом — просто инициализирует HttpClient
        public Task InitializeAsync()
        {
            HttpClient = CreateClient(); // Просто запускаем клиент, без всяких Task.Delay!
            return Task.CompletedTask;
        }

        // Вызывается после каждого [Fact] теста — Respawn мгновенно очищает таблицы
        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        // Очистка при полном закрытии тест-раннера
        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }
    }
}
