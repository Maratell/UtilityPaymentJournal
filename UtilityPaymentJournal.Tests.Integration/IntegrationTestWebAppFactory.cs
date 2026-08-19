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
using WebAppProgram = UtilityPaymentJournal.Program;

namespace UtilityPaymentJournal.Tests.Integration
{
    /// <summary>
    /// Фабрика для создания тестового веб-сервера в памяти (In-Memory TestServer).
    /// Передаем класс 'Program' вашего основного приложения, чтобы фабрика знала, как его запустить.
    /// IAsyncLifetime здесь управляет глобальным жизненным циклом (запуском и остановкой Docker).
    /// </summary>
    public class IntegrationTestWebAppFactory : WebApplicationFactory<WebAppProgram>, IAsyncLifetime
    {
        // Константа для схемы аутентификации, чтобы избежать магических строк в тестах и хандлере
        public const string AuthenticationScheme = "TestAuth";

        // глобальные статические ресурсы — общие для абсолютно всех тестов в сессии
        private static readonly PostgreSqlContainer _dbContainer;
        private static readonly DbConnection _dbConnection = null!;
        private static readonly Respawner _respawner = null!;
        private static readonly string _connectionString = null!;

        // Клиент для отправки HTTP-запросов в контроллеры. Инициализируется один раз при старте фабрики.
        public HttpClient HttpClient { get; private set; } = default!;

        /// <summary>
        /// СТАТИЧЕСКИЙ КОНСТРУКТОР — Гарантированно вызывается ОДИН РАЗ за весь прогон тестов
        /// строго в один поток, до создания каких-либо экземпляров фабрики.
        /// </summary>
        static IntegrationTestWebAppFactory()
        {
            // 1. Конфигурируем изолированный Docker-контейнер PostgreSQL для тестов
            _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
                .WithDatabase("utility_payment_journal_integration_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            // Синхронно запускаем контейнер. В статическом конструкторе нельзя использовать await,
            // поэтому применяем GetAwaiter().GetResult(). Это безопасно, так как поток здесь один.
            _dbContainer.StartAsync().GetAwaiter().GetResult();
            _connectionString = _dbContainer.GetConnectionString();

            // 2. Изолированно накатываем миграции EF Core ДО старта самого веб-сервера.
            // Это гарантирует, что к моменту первого HTTP-запроса структура БД будет полностью готова.
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);

            using (var dbContext = new ApplicationDbContext(optionsBuilder.Options, null!))
            {
                dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            }

            // 3. Открываем постоянное соединение и настраиваем Respawn для мгновенного сброса данных
            _dbConnection = new NpgsqlConnection(_connectionString);
            _dbConnection.OpenAsync().GetAwaiter().GetResult();

            _respawner = Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" } // Сбрасываем только таблицы в схеме public
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Конфигурация веб-хоста перед стартом тестового сервера.
        /// Здесь мы подменяем реальные сервисы (БД, Auth) на их тестовые аналоги.
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Сообщаем приложению, что среда выполнения — IntegrationTesting.
            // Эта строчка решает проблему фоновой миграции:
            // в файле Program.cs стоит проверка, которая отключает фоновый запуск 
            // миграций приложения, если среда равна "IntegrationTesting". Без этой строчки тестовый сервер 
            // и фабрика тестов начали бы одновременно применять миграции к одной базе данных, что вызвало бы 
            // взаимную блокировку (Deadlock) и привело к падению тестов по таймауту.

            // TODO: Решить проблему замусоривания Seq логами от интеграционных тестов.
            // Варианты решения:
            // 1. Создать appsettings.IntegrationTesting.json в основном проекте и переопределить настройки Serilog (убрать Seq-синк).
            // 2. Добавить автоматическую очистку Seq через HTTP-запрос к API (http://localhost:5341/api/events/resources/clear) в статическом конструкторе фабрики.
            // 3. Настроить жесткую Retention Policy (политику удержания данных) на 1 час в самом интерфейсе Seq.
            builder.UseEnvironment("IntegrationTesting");

            // Используем ConfigureTestServices, чтобы наши подмены гарантированно регистрировались ПОСЛЕ основного кода приложения
            builder.ConfigureTestServices(services =>
            {
                // 1. ПОДМЕНА DBCONTEXT: Удаляем конфигурацию боевой БД и регистрируем тестовую (из Docker)
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

                // 2. ПОДМЕНА АУТЕНТИФИКАЦИИ: Заменяем реальные JWT/Cookie на тестовый хандлер,
                // который будет автоматически авторизовывать запросы от лица тестового пользователя
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = AuthenticationScheme;
                    options.DefaultChallengeScheme = AuthenticationScheme;
                    options.DefaultScheme = AuthenticationScheme;
                    options.DefaultSignInScheme = AuthenticationScheme;
                    options.DefaultSignOutScheme = AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(AuthenticationScheme, options => { });

                // 3. ОТКЛЮЧЕНИЕ ФИЛЬТРОВ БЕЗОПАСНОСТИ ДЛЯ ТЕСТОВ
                services.PostConfigure<Microsoft.AspNetCore.Authorization.AuthorizationOptions>(options =>
                {
                    options.FallbackPolicy = null; // Отключаем редиректы Identity на страницу /Account/Login
                });

                services.PostConfigure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
                {
                    // Находим и удаляем фильтр автоматической валидации антиподделочных токенов (CSRF/XSRF),
                    // чтобы POST/PUT/DELETE запросы в тестах не падали с ошибкой 400 Bad Request
                    var csrfFilter = options.Filters.FirstOrDefault(f =>
                        f is Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute);

                    if (csrfFilter != null)
                    {
                        options.Filters.Remove(csrfFilter);
                    }
                });
            });
        }

        /// <summary>
        /// Вызывается xUnit автоматически ОДИН раз при создании экземпляра фабрики.
        /// Инициализирует глобальный HTTP-клиент.
        /// </summary>
        public Task InitializeAsync()
        {
            HttpClient = CreateClient();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод для ручного вызова из BaseIntegrationTest перед КАЖДЫМ тестом.
        /// Respawn за миллисекунды очищает данные во всех таблицах, сохраняя структуру таблиц и миграции.
        /// </summary>
        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        /// <summary>
        /// Вызывается xUnit автоматически, когда ВСЕ тесты завершены.
        /// Гарантирует корректное уничтожение Docker-контейнера и закрытие соединений.
        /// </summary>
        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();       // Останавливаем тестовый сервер в памяти
            await _dbConnection.DisposeAsync(); // Закрываем соединение, удерживаемое Respawner
            await _dbContainer.StopAsync();     // Останавливаем и полностью удаляем Docker-контейнер
        }
    }

    //public class IntegrationTestWebAppFactory : WebApplicationFactory<ApplicationDbContext>, IAsyncLifetime
    //{
    //    // ГЛОБАЛЬНЫЕ СТАТИЧЕСКИЕ РЕСУРСЫ — общие для всей сессии тестов
    //    private static readonly PostgreSqlContainer _dbContainer;
    //    private static readonly DbConnection _dbConnection;
    //    private static readonly Respawner _respawner;
    //    private static readonly string _connectionString;

    //    public HttpClient HttpClient { get; private set; } = default!;

    //    // СТАТИЧЕСКИЙ КОНСТРУКТОР — Вызывается гарантированно ОДИН РАЗ за весь процесс и строго в один поток
    //    static IntegrationTestWebAppFactory()
    //    {
    //        // 1. Настраиваем и синхронно запускаем контейнер
    //        _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
    //            .WithDatabase("utility_payment_journal_integration_test")
    //            .WithUsername("postgres")
    //            .WithPassword("postgres")
    //            .Build();

    //        _dbContainer.StartAsync().GetAwaiter().GetResult();
    //        _connectionString = _dbContainer.GetConnectionString();

    //        // 2. Изолированно накатываем миграции ДО старта какого-либо веб-сервера
    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        optionsBuilder.UseNpgsql(_connectionString);

    //        using (var dbContext = new ApplicationDbContext(optionsBuilder.Options, null!))
    //        {
    //            dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
    //        }

    //        // 3. Настраиваем Respawn для быстрой очистки таблиц public
    //        _dbConnection = new NpgsqlConnection(_connectionString);
    //        _dbConnection.OpenAsync().GetAwaiter().GetResult();

    //        _respawner = Respawner.CreateAsync(_dbConnection, new RespawnerOptions
    //        {
    //            DbAdapter = DbAdapter.Postgres,
    //            SchemasToInclude = new[] { "public" }
    //        }).GetAwaiter().GetResult();
    //    }

    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {
    //        // Сообщаем серверу, что среда выполнения интеграционных тестов — IntegrationTesting!
    //        builder.UseEnvironment("IntegrationTesting");

    //        builder.ConfigureTestServices(services =>
    //        {
    //            // 1. ПОДМЕНА DBCONTEXT С ХАКОМ ПРОТИВ ФОНОВЫХ МИГРАЦИЙ
    //            var dbDescriptor = services.SingleOrDefault(s =>
    //                s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

    //            if (dbDescriptor != null)
    //            {
    //                services.Remove(dbDescriptor);
    //            }

    //            services.AddDbContext<ApplicationDbContext>(options =>
    //            {
    //                options.UseNpgsql(_connectionString);
    //            });

    //            // 2. ПОДМЕНА АУТЕНТИФИКАЦИИ ДЛЯ ТЕСТОВ (ваш рабочий код)
    //            services.AddAuthentication(options =>
    //            {
    //                options.DefaultAuthenticateScheme = "TestAuth";
    //                options.DefaultChallengeScheme = "TestAuth";
    //                options.DefaultScheme = "TestAuth";
    //                options.DefaultSignInScheme = "TestAuth";
    //                options.DefaultSignOutScheme = "TestAuth";
    //            })
    //            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", options => { });

    //            // 3. ОТКЛЮЧЕНИЕ ГЛОБАЛЬНЫХ ФИЛЬТРОВ RE-DIRECT И CSRF ДЛЯ ТЕСТОВ
    //            services.PostConfigure<Microsoft.AspNetCore.Authorization.AuthorizationOptions>(options =>
    //            {
    //                options.FallbackPolicy = null; // Отключаем редиректы Identity на /account в тестах
    //            });

    //            services.PostConfigure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
    //            {
    //                // Находим и вырезаем глобальный фильтр антиподделки токенов, который генерировал 400 Bad Request
    //                var csrfFilter = options.Filters.FirstOrDefault(f =>
    //                    f is Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute);

    //                if (csrfFilter != null)
    //                {
    //                    options.Filters.Remove(csrfFilter);
    //                }
    //            });
    //        });
    //    }

    //    // Вызывается перед каждым тестовым классом — просто инициализирует HttpClient
    //    public Task InitializeAsync()
    //    {
    //        HttpClient = CreateClient(); // Просто запускаем клиент, без всяких Task.Delay!
    //        return Task.CompletedTask;
    //    }

    //    // Вызывается после каждого [Fact] теста — Respawn мгновенно очищает таблицы
    //    public async Task ResetDatabaseAsync()
    //    {
    //        await _respawner.ResetAsync(_dbConnection);
    //    }

    //    // Очистка при полном закрытии тест-раннера
    //    public new async Task DisposeAsync()
    //    {
    //        await base.DisposeAsync();
    //    }
    //}
}
