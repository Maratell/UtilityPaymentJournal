using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Filters;
using UtilityPaymentJournal.Infrastructure.ExceptionHandling;
using UtilityPaymentJournal.Infrastructure.Identity;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Mapping;
using UtilityPaymentJournal.Services;
using UtilityProviderPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Service;
using WaterReadingPaymentJournal.Mapping;
using WaterReadingPaymentJournal.Services;


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
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<DatabaseExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // <-- Добавили
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
builder.Services.AddScoped<IResidenceService, ResidenceService>();
builder.Services.AddScoped<IUtilityProviderService, UtilityProviderService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IWaterReadingService, WaterReadingService>();
builder.Services.AddScoped<IElectricityReadingService, ElectricityReadingService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();


// Регистрируем маппер как Singleton (так как в нем нет состояния)
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

builder.Services.AddControllers();

// 3. Настройка параметров куки для Identity (вместо AddCookie)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Добавление AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");


// Автоматическое применение миграций при старте контейнера
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Здесь можно логировать ошибку, если база данных еще не успела подняться
        Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
    }
}

app.Run();
