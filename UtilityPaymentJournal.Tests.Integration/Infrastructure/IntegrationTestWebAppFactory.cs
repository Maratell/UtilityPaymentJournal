using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using WebAppProgram = UtilityPaymentJournal.Program;

namespace UtilityPaymentJournal.Tests.Integration.Infrastructure
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
        /// Статический конструктор используется для гарантированной однократной инициализации 
        /// тяжелых инфраструктурных ресурсов (Docker-контейнер, миграции базы данных, Respawner) 
        /// на всю сессию запуска тестов (AppDomain).
        /// </summary>
        /// <remarks>
        /// Использование блокирующих вызовов <c>.GetAwaiter().GetResult()</c> здесь является 
        /// осознанным решением и технически необходимо, поскольку спецификация языка C# 
        /// не поддерживает асинхронные статические конструкторы (<c>async static</c>). 
        /// 
        /// Данный подход безопасен и не приводит к дедлокам (Deadlocks) или голоданию пула потоков 
        /// (Thread Pool Starvation), так как:
        /// 1. Выполняется строго один раз при инициализации типа.
        /// 2. В тестовом окружении xUnit отсутствует контекст синхронизации (SynchronizationContext).
        /// 
        /// В продакшен-коде (контроллерах, сервисах) использование <c>.GetAwaiter().GetResult()</c> 
        /// строго запрещено.
        /// </remarks>
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

            // Регистрируем глобальный хук очистки при ОСТАНОВКЕ ВСЕГО процесса тестов (AppDomain)
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                _dbConnection.Dispose();
                _dbContainer.DisposeAsync().GetAwaiter().GetResult();
            };
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

                    // Эти подмены избыточны, закомментировал
                    //options.DefaultScheme = AuthenticationScheme;
                    //options.DefaultSignInScheme = AuthenticationScheme;
                    //options.DefaultSignOutScheme = AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(AuthenticationScheme, options => { });

                // 3. ОТКЛЮЧЕНИЕ ФИЛЬТРОВ БЕЗОПАСНОСТИ ДЛЯ ТЕСТОВ
                services.PostConfigure<Microsoft.AspNetCore.Authorization.AuthorizationOptions>(options =>
                {
                    options.FallbackPolicy = null; // Отключаем редиректы Identity на страницу /Account/Login
                });

                services.PostConfigure<Microsoft.AspNetCore.Identity.IdentityOptions>(options =>
                {
                    #region НАСТРОЙКА ПРАВИЛ IDENTITY ДЛЯ ТЕСТОВ

                    // ИНЖЕНЕРНЫЙ ХАК ДЛЯ СТАТУСА NotAllowed (result.IsNotAllowed):
                    // В Microsoft Identity статус IsNotAllowed триггерится только если включено требование подтверждения аккаунта/почты.
                    // Даже если проект не использует Email (вход идет по UserName), мы ПРИНУДИТЕЛЬНО включаем флаг RequireConfirmedEmail.
                    // Это позволяет нам в тестах имитировать запрет на вход (бизнес-статус NotAllowed), просто сбрасывая 
                    // булевое поле `EmailConfirmed = false` напрямую в базе данных для нужного пользователя.
                    options.SignIn.RequireConfirmedEmail = true;

                    // СИНХРОНИЗАЦИЯ ПАРАМЕТРОВ БЛОКИРОВКИ (result.IsLockedOut):
                    // По умолчанию Identity не активирует правила блокировки для свежесозданных пользователей.
                    // Мы принудительно включаем AllowedForNewUsers, чтобы SignInManager при каждом входе проверял поле `LockoutEnd` в БД.
                    // Это позволяет честно тестировать сценарии временного бана (бизнес-статус LockedOut), переводя часы `LockoutEnd` в будущее.
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(15);

                    #endregion
                });


                services.PostConfigure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
                {
                    #region ГЛОБАЛЬНОЕ ОТКЛЮЧЕНИЕ ANTIFORGERY

                    // Удаляем автоматическую валидацию CSRF из глобальных фильтров MVC
                    // Этот код находит и удаляет AutoValidateAntiforgeryTokenAttribute, который обычно вешается на всё 
                    // приложение сразу в Program.cs. Это защищает стандартные POST/PUT/DELETE запросы в тестах.
                    //
                    // ВНИМАНИЕ: Данный шаг НЕ удаляет точечные, жестко прописанные атрибуты [ValidateAntiForgeryToken] 
                    // непосредственно на методах контроллеров (например, в AccountApiController.SignIn). 
                    // Для их нейтрализации ниже в контейнер DI регистрируется заглушка NullAntiforgery.
                    Microsoft.AspNetCore.Mvc.Filters.IFilterMetadata? csrfFilter = options.Filters.FirstOrDefault(f =>
                        f is Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute);

                    if (csrfFilter != null)
                    {
                        options.Filters.Remove(csrfFilter);
                    }

                    #endregion
                });

                #region ПОДМЕНА СЛУЖБЫ CSRF (ANTIFORGERY)

                // Отключаем валидацию точечных атрибутов [ValidateAntiForgeryToken].
                // Ищем оригинальный дескриптор службы IAntiforgery, зарегистрированный в основном приложении
                var antiforgeryDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAntiforgery));

                if (antiforgeryDescriptor != null)
                {
                    // Удаляем настоящую конфигурацию защиты из контейнера зависимостей (DI), чтобы она не перехватывала запросы
                    services.Remove(antiforgeryDescriptor);
                }

                // Регистрируем нашу тестовую заглушку StubAntiforgery в формате Singleton.
                // Теперь при обработке атрибутов [ValidateAntiForgeryToken] движок MVC будет вызывать методы нашей пустышки,
                // которая автоматически пропустит любые POST/PUT/DELETE запросы от тестового HttpClient без проверки кук.
                services.AddSingleton<IAntiforgery, StubAntiforgery>();

                #endregion
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
            await base.DisposeAsync();
        }
    }
}
