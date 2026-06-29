using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.Extensions
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
            // Заполняем даты создания для новых записей
            var addedEntities = changeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IAuditable>();

            foreach (var entity in addedEntities)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            // Заполняем даты обновления для измененных записей
            var modifiedEntries = changeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is IAuditable);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is IAuditable entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                }
            }
        }
    }
}
