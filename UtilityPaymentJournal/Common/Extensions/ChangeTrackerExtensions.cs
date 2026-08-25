using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UtilityPaymentJournal.Common.Interfaces;

namespace UtilityPaymentJournal.Common.Extensions
{
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        /// Автоматическое связывание новых сущностей с текущим пользователем.
        /// </summary>
        public static void ApplyUserOwnership(this ChangeTracker changeTracker, string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            var addedEntries = changeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IUserOwned>();

            foreach (var entity in addedEntries)
            {
                entity.UserId = userId;
            }
        }

        /// <summary>
        /// Автоматическое заполнение дат создания и обновления данных без вложенных условий.
        /// </summary>
        public static void ApplyAuditDates(this ChangeTracker changeTracker)
        {
            // Фиксируем единое время для всей транзакции
            DateTime currentTime = DateTime.UtcNow;

            // Для новых записей задаем и CreatedAt, и UpdatedAt
            IEnumerable<IAuditable> addedEntities = changeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IAuditable>();

            foreach (var entity in addedEntities)
            {
                entity.CreatedAt = currentTime;
                entity.UpdatedAt = currentTime;
            }

            // Для измененных записей обновляем только UpdatedAt и защищаем CreatedAt
            IEnumerable<EntityEntry> modifiedEntries = changeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is IAuditable);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is IAuditable entity)
                {
                    entity.UpdatedAt = currentTime;

                    // Защита: этой строчкой сообщаем EF Core: Даже если значение свойства CreatedAt изменилось
                    // в памяти приложения, не записывай его в базу данных и проигнорируй это изменение
                    entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                }
            }
        }
    }
}
