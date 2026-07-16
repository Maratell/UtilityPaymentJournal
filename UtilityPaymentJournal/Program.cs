using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using System.Security.Claims;
using UtilityPaymentJournal.Common.Constants;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.ExceptionHandling;
using UtilityPaymentJournal.Infrastructure.Identity;
using UtilityPaymentJournal.Infrastructure.JsonConverters;
using UtilityPaymentJournal.Infrastructure.Middlewares;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Mapping;
using UtilityPaymentJournal.Services;
using UtilityProviderPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Mapping;


var builder = WebApplication.CreateBuilder(args);

// Настрйока культуры (для корректной работы значений с плавающей точкой)
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("ru-RU") };

    // Говорим серверу: форматы дат и строк оставляем русскими, 
    // но парсинг чисел (NumberFormat) делаем инвариантным (всегда с точкой '.')
    foreach (var culture in supportedCultures)
    {
        culture.NumberFormat.NumberDecimalSeparator = ".";
        culture.NumberFormat.CurrencyDecimalSeparator = ".";
    }

    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


// регистрация глобальной обработки ошибок
builder.Services.AddProblemDetails();

// Порядок регистрации критически важен! Общий обработчик ВСЕГДА идет самым последним.
builder.Services.AddExceptionHandler<IdentityValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<DatabaseExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// -------------------------------------------------

//builder.Services.AddControllersWithViews();

// Регистрируем контроллеры с поддержкой представлений (Razor-страниц) и настраиваем фильтры
builder.Services.AddControllersWithViews(options =>
{
    // Фильтр автоматической проверки входящих CSRF / Antiforgery токенов.
    // Защищает абсолютно все методы POST, PUT, DELETE в проекте, избавляя от рутины.
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

//Add services to the container.
//builder.Services.AddControllersWithViews(options =>
//{
//    // Фильтр работает для всех контроллеров приложения
//    options.Filters.Add<ValidateModelAttribute>();
//});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResidenceService, ResidenceService>();
builder.Services.AddScoped<IUtilityProviderService, UtilityProviderService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IWaterReadingService, WaterReadingService>();
builder.Services.AddScoped<IElectricityReadingService, ElectricityReadingService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
// Регистрация Middleware для добавления ID пользователя в контекст логирования
builder.Services.AddScoped<UserLoggingMiddleware>();


// Регистрируем маппер как Singleton (так как в нем нет состояния)
builder.Services.AddScoped<IAccountMapper, AccountMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddSingleton<IResidenceMapper, ResidenceMapper>();
builder.Services.AddSingleton<IUtilityProviderMapper, UtilityProviderMapper>();
builder.Services.AddSingleton<IUtilityMapper, UtilityMapper>();
builder.Services.AddSingleton<IWaterReadingMapper, WaterReadingMapper>();
builder.Services.AddSingleton<IElectricityReadingMapper, ElectricityReadingMapper>();
builder.Services.AddSingleton<IComplaintMapper, ComplaintMapper>();

// Позволяет получать HttpContext (и claims пользователя) внутри классов данных
builder.Services.AddHttpContextAccessor();


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login"; // Страница входа
//        options.Cookie.HttpOnly = true;

//        // ВАЖНО ДЛЯ РАЗРАБОТКИ: 
//        // SameSiteMode.Lax разрешает передачу куки при переходе по ссылке
//        options.Cookie.SameSite = SameSiteMode.Lax;

//        // Предотвращает блокировку куки, если вы тестируете через http://
//        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
//    });

//// ??
//builder.Services.AddControllersWithViews();

// Регистрируем контекст с провайдером PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// внедряем в проект систему ASP.NET Core Identity для управления пользователями, ролями и безопасностью
builder.Services.AddIdentity<User, Role>(options =>
{
    // Настройки сложности пароля
    options.Password.RequiredLength = 4;        // Минимальная длина (например, 4 символа)
    options.Password.RequireDigit = false;       // Отключить обязательные цифры
    options.Password.RequireLowercase = false;   // Отключить обязательные строчные буквы
    options.Password.RequireUppercase = false;   // Отключить обязательные заглавные буквы
    options.Password.RequireNonAlphanumeric = false; // Отключить спецсимволы
    options.Password.RequiredUniqueChars = 1;    // Количество уникальных символов
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    // Подключаем кастомную фабрику клеймов
    .AddClaimsPrincipalFactory<UserProfileClaimsPrincipalFactory>();

// Включаем глобальную блокировку: по умолчанию неавторизованные пользователи не могут
// отправлять запросы
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Регистрируем контроллеры API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Подключаем кастомный конвертер для корректного отображения DateTime и DateTime?.
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableUtcDateTimeJsonConverter());
    });

// Настройка параметров куки для Identity (вместо AddCookie)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Добавление AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Подключаем Serilog и заставляем его читать appsettings.json
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Глобальная настройка для драйвера базы данных PostgreSQL (Npgsql).
// Принудительно заставляет .NET помечать все даты, выгружаемые из колонок 'timestamptz', 
// как DateTimeKind.Utc, исключая появление типа 'Unspecified'.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

var app = builder.Build();

// Собирает данные о HTTP-запросе в один структурированный JSON-объект для Seq
// (сохраняет метод, URL, статус-код и скорость ответа как отдельные поля для поиска)
app.UseSerilogRequestLogging(options =>
{
    // Обогащает финальный лог ответа (например: "HTTP GET /residences responded 200") данными (ID) пользователя.
    // Это необходимо, так как подобный системный лог записывается вне зоны видимости кастомного UserLoggingMiddleware.
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        string? userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            diagnosticContext.Set(LogPropertyConstants.UserId, userId);
        }
    };
});

// Активирует централизованную обработку ошибок. Все исключения из контроллеров и сервисов 
// будут поочередно проходить через кастомные обработчики (NotFound, Database, Global), 
// возвращая клиенту стандартизированный ответ ProblemDetails вместо аварийного падения.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Сперва подключаем аутентификацию (кем является пользвователь?) и авторизацию (какие у него права)
app.UseAuthentication();
app.UseAuthorization();

// Затем обогащаем логи данными пользователя (он уже распознан системой Identity)
app.UseMiddleware<UserLoggingMiddleware>();

app.MapControllers(); // Позволит атрибутам [Route(...)] работать на 100% правильно
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

// Автоматическое применение миграций при старте контейнера
//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        db.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        // Здесь можно логировать ошибку, если база данных еще не успела подняться
//        Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
//    }
//}
_ = Task.Run(async () =>
{
    // Небольшая пауза, чтобы Kestrel и Seq успели занять порты на старте
    await Task.Delay(TimeSpan.FromSeconds(2));

    using var scope = app.Services.CreateScope();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
        Console.WriteLine("=== База данных успешно проверена, миграции применены ===");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"=== КРИТИЧЕСКАЯ ОШИБКА МИГРАЦИИ БД: {ex.Message} ===");
    }
});

app.Run();


