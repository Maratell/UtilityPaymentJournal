using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using UtilityPaymentJournal.Common.Behaviours;
using UtilityPaymentJournal.Common.Constants;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Features.Users.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.ExceptionHandling;
using UtilityPaymentJournal.Infrastructure.Filters;
using UtilityPaymentJournal.Infrastructure.Identity;
using UtilityPaymentJournal.Infrastructure.JsonConverters;
using UtilityPaymentJournal.Infrastructure.Middlewares;


// Инициализируем базовый сборщик (Builder) веб-приложения.
// Он отвечает за загрузку файлов конфигурации (appsettings.json), настройку логирования,
// параметров веб-сервера Kestrel и регистрацию всех зависимостей (Dependency Injection).
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region НАСТРОЙКА ЛОКАЛИЗАЦИИ (Культура и форматы данных)
// Настройка локализации приложения (для корректной работы чисел с плавающей точкой)
// Мы принудительно заставляем сервер использовать точку '.' вместо запятой ',' как разделитель в дробных числах.
// Это критически важно для работы API, чтобы JSON-запросы с децимал/флоат значениями (например, 10.5) не падали с ошибками парсинга.
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    CultureInfo[] supportedCultures = new[] { new CultureInfo("ru-RU") };

    // Говорим серверу: форматы дат и строк оставляем русскими, 
    // но парсинг чисел (NumberFormat) делаем инвариантным (всегда с точкой '.')
    foreach (CultureInfo culture in supportedCultures)
    {
        culture.NumberFormat.NumberDecimalSeparator = ".";
        culture.NumberFormat.CurrencyDecimalSeparator = ".";
    }

    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
#endregion

#region РЕГИСТРАЦИЯ ОБРАБОТКИ ОШИБОК (Exception Handling)

builder.Services.AddProblemDetails();

// РЕГИСТРАЦИЯ ОБРАБОТЧИКОВ ИСКЛЮЧЕНИЙ (IExceptionHandler)
// Порядок регистрации критически важен! Конвейер перехвата ошибок работает по принципу цепочки:
// запрос идет сверху вниз. Как только один из хендлеров вернет true, обработка завершается.
// Поэтому специфичные (узкие) ошибки ловим первыми, а универсальный GlobalExceptionHandler ВСЕГДА идет самым последним.
builder.Services.AddExceptionHandler<IdentityValidationExceptionHandler>();  // Ошибки валидации ASP.NET Core Identity
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>(); // Ошибки отсутствия ресурсов (404)
builder.Services.AddExceptionHandler<DatabaseExceptionHandler>(); // Сбои и исключения при работе с базой данных
builder.Services.AddExceptionHandler<ValidationExceptionHandler>(); // Общие ошибки валидации моделей (FluentValidation и т.д.)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Корневой «улавливатель» для всех непредвиденных системных сбоев (500)

#endregion

#region РЕГИСТРАЦИЯ АРХИТЕКТУРНЫХ КОМПОНЕНТОВ (MVC, FluentValidation, MediatR)

// Подключаем поддержку контроллеров и Razor-страниц (Views) с глобальной конфигурацией фильтров
builder.Services.AddControllersWithViews(options =>
{
    // Глобальный фильтр автоматической проверки CSRF / Antiforgery токенов.
    // Он автоматически защищает все изменяющие методы (POST, PUT, PATCH, DELETE) во всем проекте,
    // избавляя от необходимости вручную расставлять [ValidateAntiForgeryToken] над каждым эндпоинтом.
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// FluentValidation: Автоматически сканируем текущую сборку (Assembly) 
// и регистрируем все созданные нами классы валидаторов (наследников AbstractValidator<T>) в DI-контейнере.
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// НАСТРОЙКА МЕДИАТОРА (Паттерн CQRS / Mediator)
builder.Services.AddMediatR(cfg =>
{
    // Сканируем текущую сборку для автоматического поиска и регистрации всех Request/Response хэндлеров
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    // Встраиваем валидацию в сквозной пайплайн MediatR (Мiddleware-поведение).
    // Этот код гарантирует, что валидация бизнес-правил выполнится ДО того, как запрос попадет в основной хэндлер.
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

#endregion

#region РЕГИСТРАЦИЯ СЕРВИСОВ ПРИЛОЖЕНИЯ И КОНТЕКСТА ПОЛЬЗОВАТЕЛЯ (Application Services & Context)

// Сервис для выполнения безопасных (Read-Only) запросов к данным пользователей
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// Сервис для получения данных о текущем залогиненном пользователе (ID, Имя, Роли) в любом слое приложения
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Регистрация кастомного Middleware, которое внедряет ID текущего пользователя в контекст логирования (Serilog + Seq).
// Зарегистрировано как Scoped, так как оно зависит от HttpContext конкретного входящего запроса.
builder.Services.AddScoped<UserLoggingMiddleware>();

// Системный сервис ASP.NET Core, позволяющий получать доступ к HttpContext (и Claims пользователя) 
// внутри обычных классов, сервисов или слоев данных, где HttpContext недоступен напрямую.
// Метод расширения автоматически регистрирует данный компонент как Singleton в DI-контейнере.
builder.Services.AddHttpContextAccessor();

#endregion

#region КОНФИГУРАЦИЯ БАЗЫ ДАННЫХ (PostgreSQL & EF Core)

// Извлекаем строку подключения из файла конфигурации (appsettings.json)
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Регистрируем контекст базы данных Entity Framework Core в DI-контейнере.
// По умолчанию AddDbContext регистрирует контекст как Scoped (один экземпляр на один HTTP-запрос).
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Связываем интерфейс IApplicationDbContext с реальной реализацией ApplicationDbContext.
// Используем фабричный метод фабрики, чтобы возвращать ТОТ ЖЕ самый экземпляр контекста (Scoped), 
// который уже был создан выше (через AddDbContext), предотвращая создание дублирующих подключений в рамках одного запроса.
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

#endregion

#region КОНФИГУРАЦИЯ БЕЗОПАСНОСТИ (ASP.NET Core Identity & Глобальная Авторизация)

// Внедряем систему ASP.NET Core Identity для управления пользователями, ролями и сессиями
builder.Services.AddIdentity<User, Role>(options =>
{
    // НАСТРОЙКИ СЛОЖНОСТИ ПАРОЛЯ (Password Requirements)
    // Облегченные настройки для удобства локальной разработки и тестирования
    options.Password.RequiredLength = 4;             // Минимальная длина пароля — 4 символа
    options.Password.RequireDigit = false;           // Отключаем обязательные цифры
    options.Password.RequireLowercase = false;       // Отключаем обязательные строчные буквы
    options.Password.RequireUppercase = false;       // Отключаем обязательные заглавные буквы
    options.Password.RequireNonAlphanumeric = false; // Отключаем спецсимволы
    options.Password.RequiredUniqueChars = 1;        // Минимальное количество уникальных символов в пароле

    // НАСТРОЙКИ БЛОКИРОВКИ АККАУНТА (User Lockout)
    // Защита от перебора паролей (Brute-Force атак)
    options.Lockout.AllowedForNewUsers = true;      // Включать блокировку для всех новых аккаунтов
    options.Lockout.MaxFailedAccessAttempts = 5;    // Блокировать учетную запись после 5 неудачных попыток входа
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Время блокировки — 15 минут
})
    // Указываем Identity использовать наш Entity Framework контекст для хранения таблиц пользователей и ролей
    .AddEntityFrameworkStores<ApplicationDbContext>()
    // Подключаем стандартные провайдеры токенов (нужны для генерации кодов сброса пароля, подтверждения Email и т.д.)
    .AddDefaultTokenProviders()
    // Подключаем кастомную фабрику утверждений (Claims). Она обогащает объект пользователя (ClaimsPrincipal) 
    // дополнительными данными при авторизации, которые затем доступны во всем приложении через HttpContext.
    .AddClaimsPrincipalFactory<UserProfileClaimsPrincipalFactory>();

// НАСТРОЙКА ПОЛИТИК АВТОРИЗАЦИИ (Authorization Policies)
builder.Services.AddAuthorization(options =>
{
    // Включаем строгую глобальную политику защиты (Secure by Default).
    // Если над контроллером или эндпоинтом НЕ стоит атрибут [AllowAnonymous], 
    // система автоматически потребует от пользователя быть авторизованным для любого действия.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

#endregion

#region СЕРВИСЫ: Контроллеры API и Настройка Сериализации JSON

// Регистрируем контроллеры API с кастомной настройкой сериализации JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Подключаем кастомный конвертер для корректного отображения DateTime и DateTime?.
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableUtcDateTimeJsonConverter());
    });

// Подключаем исследователь эндпоинтов (необходим для корректного обнаружения Minimal API эндпоинтов в Swagger)
builder.Services.AddEndpointsApiExplorer();

#endregion

#region СЕРВИСЫ: Конфигурация Защиты от CSRF (Antiforgery)

// Настраиваем глобальный механизм защиты от межсайтовой подделки запросов (CSRF)
builder.Services.AddAntiforgery(options =>
{
    // Сервер строго завязан на развернутую константу безопасности.
    // Имя заголовка (AntiforgerySecurityConstants.AntiforgeryHeaderName)
    // полностью синхронизировано со Swagger-фильтром и фронтендом.
    options.HeaderName = AntiforgerySecurityConstants.AntiforgeryHeaderName;
});

#endregion

#region СЕРВИСЫ: Глубокая Настройка Авторизационных Кук (Identity Application Cookie)

// Настройка параметров Cookie-сессии, создаваемой системой Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    // НАСТРОЙКИ СВОЙСТВ COOKIE (Cookie Policies & Security)
    options.LoginPath = "/account"; // Адрес перенаправления, если неавторизованный пользователь пытается открыть защищенную страницу
    options.Cookie.HttpOnly = true; // Флаг защиты от XSS-атак (Cross-Site Scripting): запрещает чтение куки через JavaScript скрипты
    options.Cookie.SameSite = SameSiteMode.Lax; // Защита от CSRF (Cross-Site Request Forgery): запрещает отправку куки при скрытых запросах со сторонних сайтов
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;// Подстраивает режим передачи куки под текущий запрос: если сайт открыт по http — кука идет без шифрования, если по https — с шифрованием

    // НАСТРОЙКИ ВРЕМЕНИ ЖИЗНИ СЕССИИ (Session Lifetime)
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Срок действия сессии пользователя при его полной неактивности
    options.SlidingExpiration = true; // Автоматически продлевает сессию еще на 60 минут при каждом новом действии пользователя

    // ПЕРЕХВАТ СОБЫТИЙ КОНВЕЙЕРА БЕЗОПАСНОСТИ (Security Events & API Handlers)
    // Изменение поведения при ошибке 401 Unauthorized (Пользователь не залогинен)
    options.Events.OnRedirectToLogin = context =>
    {
        // Разделяем поведение для классического фронтенда и API/Swagger.
        // Если запрос идет на REST API или документацию, редирект не имеет смысла.
        if (context.Request.Path.StartsWithSegments("/api") ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // Возвращаем честный HTTP-статус 401 Unauthorized, понятный для Swagger UI и AJAX-запросов
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            // Для обычных пользователей, переходящих по ссылкам в браузере, оставляем стандартный редирект на форму входа
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    // Изменение поведения при ошибке 403 Forbidden (Пользователь залогинен, но у него нет нужной роли/прав)
    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api") ||
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            // Вместо редиректа возвращаем честный REST-статус 403 Forbidden
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
});

#endregion

// Закомментировал AutoMapper
//builder.Services.AddAutoMapper(typeof(Program));


#region СЕРВИСЫ: Генерация Документации API (Swagger UI)

// Конфигурация генератора документации Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Подключаем кастомный фильтр операций. 
    // Он автоматически добавит поле ввода CSRF-токена (AntiforgerySecurityConstants.AntiforgeryHeaderName) для всех защищенных методов.
    options.OperationFilter<SwaggerAntiforgeryFilter>();

    // Решает конфликт одинаковых имен классов из разных пространств имен (например, вложенных классов Item).
    // По умолчанию Swagger использует только имя класса, из-за чего одинаковые имена вызывают ошибку компиляции спецификации.
    // Данная лямбда заменяет имена на полные (FullName), превращая системный разделитель вложенности "+" в читаемый символ "_".
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));

    // Говорим Swagger корректно отображать системный тип CancellationToken как пустой объект,
    // чтобы он не засорял интерфейс Swagger UI внутренними свойствами токена отмены в каждом запросе.
    options.MapType<CancellationToken>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.Object
    });

    // Явно указываем спецификации Swagger, как отображать DateTime в формате ISO (строка с date-time)
    options.MapType<DateTime>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.String,
        Format = "date-time"
    });

    // Аналогично настраиваем отображение для Nullable-типа DateTime?
    options.MapType<DateTime?>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.String,
        Format = "date-time",
    });
});

#endregion

#region СЕРВИСЫ: Системное Логирование (Serilog)

// Подключаем Serilog в качестве основного провайдера логирования приложения.
// Метод ReadFrom.Configuration заставляет логер автоматически считывать уровни логирования,
// пути к файлам и настройки вывода (Sinks) напрямую из файла конфигурации appsettings.json.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

#endregion

#region СИСТЕМНЫЕ НАСТРОЙКИ: Инициализация Окружения Среды (.NET / Npgsql)

// Глобальная настройка для драйвера базы данных PostgreSQL (Npgsql).
// Отключаем старое (Legacy) поведение работы с датами. Это принудительно заставляет .NET помечать все даты, 
// выгружаемые из колонок типа 'timestamp with time zone' (timestamptz), как DateTimeKind.Utc.
// Исключает появление опасного типа 'DateTimeKind.Unspecified', который часто приводит к сдвигу времени на продакшене.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

#endregion

WebApplication app = builder.Build();

#region КОНВЕЙЕР (MIDDLEWARE): Сквозное структурированное Логирование (Serilog)

// Перехватывает каждый входящий запрос и собирает все данные о нем в один структурированный JSON-объект.
// Автоматически фиксирует HTTP-метод, URL, статус-код ответа и точную скорость его выполнения в миллисекундах.
app.UseSerilogRequestLogging(options =>
{
    // Обогащает системный лог завершения запроса данными пользователя - UserId.
    // Это критически важно, так как стандартные логи Kestrel записываются на самом выходе из конвейера, 
    // где наше кастомное Scoped-middleware (UserLoggingMiddleware) уже уничтожено.
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        // Ищем уникальный идентификатор пользователя (ID) в его Claims (утверждениях)
        string? userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            // Добавляем ID пользователя как отдельное индексируемое поле для удобного поиска в Seq
            diagnosticContext.Set(LogPropertyConstants.UserId, userId);
        }
    };

    
});

#endregion

#region КОНВЕЙЕР (MIDDLEWARE): Глобальный Перехват и Обработка Ошибок
// Активирует встроенный централизованный конвейер обработки исключений. 
// Все непредвиденные ошибки из контроллеров и хэндлеров MediatR будут автоматически перехватываться
// и передаваться по цепочке в наши кастомные хэндлеры (NotFoundExceptionHandler, DatabaseExceptionHandler и т.д.),
// возвращая клиенту стандартизированный JSON-ответ в формате RFC 7807 Problem Details вместо аварийного падения приложения.

app.UseExceptionHandler();

#endregion

#region КОНВЕЙЕР (MIDDLEWARE): Специфичные Настройки для Production Среды

// Данный блок настроек безопасности активируется только при работе приложения на реальном сервере (Production)
if (!app.Environment.IsDevelopment())
{
    // Активирует протокол безопасности HTTP Strict Transport Security (HSTS).
    // Принудительно заставляет браузеры клиентов взаимодействовать с нашим сайтом исключительно по защищенному протоколу HTTPS,
    // защищая от атак типа Man-in-the-Middle (MITM) и перехвата незашифрованного трафика.
    app.UseHsts();
}

#endregion

#region КОНВЕЙЕР (MIDDLEWARE): Специфичные Настройки для Локальной Разработки (Development)

// Данный блок кода выполняется исключительно в режиме отладки и разработки. Полностью вырезается на Production.
if (app.Environment.IsDevelopment())
{
    // Включаем генерацию JSON-документации API
    app.UseSwagger(); 

    // Настраиваем графический интерактивный интерфейс Swagger UI
    app.UseSwaggerUI(options =>
    {
        // Указываем путь к сгенерированному файлу спецификации OpenAPI
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

        // Для удобства разработки: Swagger сохраняет введенные токены авторизации и куки при обновлении страницы (F5)
        options.EnablePersistAuthorization(); 
    });

    // Вспомогательный Minimal API эндпоинт для автоматического редиректа: 
    // при вводе в адресную строку просто "/swagger", приложение мгновенно перенаправляет 
    // разработчика на полноценную страницу графического интерфейса "/swagger/index.html".
    // Разрешаем анонимный доступ (.AllowAnonymous), чтобы глобальная FallbackPolicy безопасности не блокировала UI.
    app.MapGet("/swagger", (HttpContext context) =>
    {
        context.Response.Redirect("/swagger/index.html");
    }).AllowAnonymous();

    // Специальный вспомогательный Minimal API эндпоинт для генерации и получения CSRF-токена безопасности.
    // Из него мы копируем текстовое значение токена и вставляем его в поле RequestVerificationToken в Swagger UI,
    // чтобы успешно тестировать изменяющие данные методы (POST, PUT, DELETE), защищенные фильтром AutoValidate.
    // Доступен анонимно, так как без этого токена невозможно выполнить даже метод входа в систему (SignIn).
    app.MapGet("/api/xsrf-token", (HttpContext context, Microsoft.AspNetCore.Antiforgery.IAntiforgery antiforgery) =>
    {
        Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
        return Results.Ok(new { token = tokens.RequestToken });
    })
    .WithName("GetXsrfToken")
    .AllowAnonymous();
}

#endregion

#region КОНВЕЙЕР (MIDDLEWARE): Маршрутизация, Локализация и Безопасность Запросов

// Применяем настройки локализации (наша инвариантная точка '.' в числах будет работать для всех входящих запросов)
app.UseRequestLocalization();

// Автоматически перенаправляет все незащищенные HTTP-запросы на безопасный протокол HTTPS
app.UseHttpsRedirection();

// Разрешаем раздачу статических файлов (CSS, JS, картинки) из папки wwwroot для Razor-страниц
app.UseStaticFiles();

// Включаем сопоставление маршрутов (Routing) — соотносит URL запроса с конкретным эндпоинтом приложения
app.UseRouting();

// СИСТЕМА БЕЗОПАСНОСТИ (Identity Pipeline)
// Важно: Порядок вызовов строго зафиксирован!
app.UseAuthentication(); // 1. Аутентификация: Проверяем куки/токены и определяем, КЕМ является пользователь
app.UseAuthorization();  // 2. Авторизация: Проверяем, КАКИЕ права и роли есть у этого распознанного пользователя

// Создаем эндпоинт, по которому Swagger UI скачивает файл разметки (swagger.json).
// Метод .AllowAnonymous() строго необходим, чтобы этот файл открывался без ввода логина и пароля.
// Если его не написать, наша глобальная защита заблокирует этот JSON, и сам интерфейс Swagger просто не загрузится.
app.MapSwagger("/swagger/{documentName}/swagger.json").AllowAnonymous();

// МОНИТОРИНГ И ЛОГИРОВАНИЕ
// Подключаем наше кастомное Middleware для обогащения логов.
// Оно стоит ПОСЛЕ авторизации, поэтому система Identity уже расшифровала куку и мы можем достать ID пользователя.
app.UseMiddleware<UserLoggingMiddleware>();

#endregion

#region КОНВЕЙЕР (MIDDLEWARE): Регистрация Маршрутов Контроллеров (Endpoints)

// Маппим API-контроллеры: это позволяет атрибутам [Route(...)] и [HttpPost]/[HttpGet] на эндпоинтах работать на 100% правильно
app.MapControllers();

// Настраиваем классический шаблон маршрутизации по умолчанию для Razor-страниц (MVC UI).
// Если пользователь откроет корень сайта, его автоматически направит на AccountController и эндпоинт Index.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

#endregion

#region ИНИЦИАЛИЗАЦИЯ ПРИЛОЖЕНИЯ: Автоматические Миграции БД на старте (Только вне тестов)

// Защита конвейера тестирования: если приложение запущено внутри интеграционных тестов, 
// автоматические миграции не запускаются (тесты используют свою изолированную или in-memory базу данных)
if (app.Environment.EnvironmentName != "IntegrationTesting")
{
    // Запускаем миграции в фоновой задаче (Task), чтобы не блокировать последовательную 
    // инициализацию конвейера и запуск самого веб-сервера Kestrel (app.Run()).
    // Это гарантирует, что сервер мгновенно откроет порты на старте, а миграции накатятся параллельно.
    _ = Task.Run(async () =>
    {
        // Небольшая пауза (2 секунды). Это критически важно при запуске в Docker Compose, 
        // чтобы Kestrel и сборщик логов успели занять порты, а СУБД PostgreSQL успела полностью инициализироваться.
        await Task.Delay(TimeSpan.FromSeconds(2));

        // Создаем изолированную область видимости (Scope) для безопасного извлечения Scoped-сервисов на старте приложения
        using IServiceScope scope = app.Services.CreateScope();
        try
        {
            ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Автоматически применяем все недостающие миграции к базе данных PostgreSQL
            await db.Database.MigrateAsync();

            // Текст написал по-английски для корректного отображения в powershell
            Console.WriteLine("=== Database successfully verified, migrations applied ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=== CRITICAL DATABASE MIGRATION ERROR: {ex.Message} ===");
        }
    });
}

// Главный терминальный компонент приложения. Запускает веб-сервер Kestrel, 
// открывает сетевые порты и переводит приложение в режим бесконечного ожидания 
// и обработки входящих HTTP-запросов. Полностью блокирует дальнейший поток выполнения.
app.Run();

#endregion

#region ТЕСТИРОВАНИЕ: Доступ к сборке для интеграционных тестов

namespace UtilityPaymentJournal
{
    // По умолчанию компилятор делает класс Program внутренним (internal).
    // Данный partial-класс принудительно делает его публичным (public).
    // Это строго необходимо, чтобы тестовый проект (xUnit/NUnit) мог увидеть сборку приложения 
    // через WebApplicationFactory<Program> и запускать полноценные интеграционные тесты.
    public partial class Program { }
}

#endregion



