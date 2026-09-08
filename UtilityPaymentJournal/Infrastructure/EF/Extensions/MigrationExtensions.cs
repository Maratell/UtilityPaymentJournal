using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Constants;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Infrastructure.EF.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            // Защита конвейера тестирования: если приложение запущено внутри интеграционных тестов, 
            // автоматические миграции не запускаются (тесты используют свою изолированную или in-memory базу данных)
            if (app.Environment.EnvironmentName == "IntegrationTesting")
            {
                return;
            }

            // Выполняем миграции последовательно до запуска Kestrel.
            // Это гарантирует, что порты откроются только тогда, когда база данных на 100% готова.
            using (IServiceScope scope = app.Services.CreateScope())
            {
                int retries = DatabaseConstants.MaxMigrationRetries; // 6 попыток с паузой в 5 секунд = 30 секунд общего времени ожидания СУБД
                while (retries > 0)
                {
                    try
                    {
                        Console.WriteLine("=== Checking database connection and applying migrations... ===");
                        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        // Автоматически применяем все недостающие миграции к базе данных PostgreSQL
                        await context.Database.MigrateAsync();

                        Console.WriteLine("=== Database successfully verified, migrations applied ===");
                        break; // Успешно применили миграции, выходим из цикла ожидания
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        Console.WriteLine($"=== DATABASE NOT READY YET. Retries left: {retries}. Error: {ex.Message} ===");

                        if (retries == 0)
                        {
                            Console.WriteLine("=== CRITICAL DATABASE MIGRATION ERROR: Could not connect to PostgreSQL ===");
                            throw; // Приложение завершает работу, Docker перезапустит контейнер
                        }

                        // Пауза перед следующей попыткой. Критически важно для Docker Compose,
                        // пока контейнер PostgreSQL инициализирует свои внутренние папки.
                        await Task.Delay(TimeSpan.FromSeconds(DatabaseConstants.MigrationDelaySeconds));
                    }
                }
            }
        }
    }
}
