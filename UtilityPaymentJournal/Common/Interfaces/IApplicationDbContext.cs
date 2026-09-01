using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Common.Interfaces
{
    /// <summary>
    /// Интерфейс-контракт для работы с базой данных приложения.
    /// Позволяет слою бизнес-логики (Application/Handlers) использовать таблицы и транзакции,
    /// не привязываясь к конкретной реализации Entity Framework, что делает код гибким 
    /// и позволяет легко писать легковесные Unit-тесты без Docker и реальной БД.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Таблица жилых объектов.
        /// </summary>
        DbSet<Residence> Residences { get; }

        /// <summary>
        /// Таблица коммунальных услуг.
        /// </summary>
        DbSet<Utility> Utilities { get; }

        /// <summary>
        /// Таблица поставщиков коммунальных услуг (управляющие компании, водоканалы).
        /// </summary>
        DbSet<UtilityProvider> UtilityProviders { get; }

        /// <summary>
        /// Таблица показаний счетчиков (холодной, горячей воды и водоотведения).
        /// </summary>
        DbSet<WaterReading> WaterReadings { get; }

        /// <summary>
        /// Таблица показаний счетчиков электроэнергии.
        /// </summary>
        DbSet<ElectricityReading> ElectricityReadings { get; }

        /// <summary>
        /// Таблица жалоб.
        /// </summary>
        DbSet<Complaint> Complaints { get; }

        /// <summary>
        /// Доступ к управлению низкоуровневыми механизмами базы данных.
        /// Вынесен в интерфейс, чтобы можно было использовать в обработчиках (Handler),
        /// а в Unit-тестах — легко симулировать успешные фиксации (Commit) или аварийные откаты (Rollback) данных.
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Асинхронное сохранение всех изменений, сделанных в таблицах, в постоянное хранилище (БД).
        /// Основной рабочий метод для фиксации изменений (создание, обновление, удаление записей).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции (например, если пользователь закрыл вкладку в браузере).</param>
        /// <returns>Количество строк, которые были успешно изменены или добавлены в базу данных.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Синхронное сохранение всех изменений в базу данных.
        /// Используется реже асинхронного, в основном там, где невозможно или не требуется разворачивать асинхронный контекст.
        /// </summary>
        /// <returns>Количество строк, измененных в базе данных.</returns>
        int SaveChanges();
    }
}
