using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Перевессти имена таблиц бд в нижний регистр (для удобства запросов к Postgre SQL)
        /// </summary>
        /// <param name="builder"></param>
        public static void UseLowerCaseNamingConvention(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(currentTableName))
                {
                    entity.SetTableName(currentTableName.ToLowerInvariant());
                }
            }
        }

        /// <summary>
        /// Логика фильтра запросов по Id пользователя (не нужно прописывать каждую сущность отдельно, собирает все IUserOwned)
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="httpContextAccessor"></param>
        public static void ApplyUserGlobalFilters(this ModelBuilder modelBuilder, Func<string?> getCurrentUserId)
        {
            // Находим все сущности в модели БД, которые реализуют интерфейс IUserOwned
            var userOwnedEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IUserOwned).IsAssignableFrom(e.ClrType));

            foreach (var entityType in userOwnedEntityTypes)
            {
                // Передаем сам делегат getCurrentUserId, а не готовую строку!
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType, getCurrentUserId));
            }

            //modelBuilder.Entity<Residence>().HasQueryFilter(r => r.UserId == userId);
            //modelBuilder.Entity<Utility>().HasQueryFilter(u => u.UserId == userId);
            //modelBuilder.Entity<UtilityProvider>().HasQueryFilter(p => p.UserId == userId);
            //modelBuilder.Entity<UtilityProviderLink>().HasQueryFilter(upl => upl.UserId == userId);
        }


        private static System.Linq.Expressions.LambdaExpression ConvertFilterExpression(Type entityType, Func<string?> getCurrentUserId)
        {
            // Строим выражение: e => e.UserId == getCurrentUserId()
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
            var property = System.Linq.Expressions.Expression.Property(parameter, nameof(IUserOwned.UserId));

            // Получаем доступ к вызову метода getCurrentUserId.Invoke()
            var invokeCall = System.Linq.Expressions.Expression.Invoke(
                System.Linq.Expressions.Expression.Constant(getCurrentUserId),
                null
            );

            var equality = System.Linq.Expressions.Expression.Equal(property, invokeCall);
            return System.Linq.Expressions.Expression.Lambda(equality, parameter);
        }
    }
}
