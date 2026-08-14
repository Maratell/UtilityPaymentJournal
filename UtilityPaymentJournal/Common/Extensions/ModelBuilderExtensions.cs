using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Infrastructure.Identity;
using UtilityPaymentJournal.Interface.Entity;

namespace UtilityPaymentJournal.Common.Extensions
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
        /// Автоматически сканирует всю объектную модель базы данных (Metadata) при старте приложения, 
        /// находит все таблицы с поддержкой интерфейса IUserOwned и централизованно применяет к ним 
        /// динамический глобальный фильтр запросов (Global Query Filter).
        /// </summary>
        /// <param name="modelBuilder">Системный конструктор моделей Entity Framework Core, используемый внутри OnModelCreating.</param>
        public static void ApplyUserGlobalFilters(this ModelBuilder modelBuilder)
        {
            var userOwnedEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IUserOwned).IsAssignableFrom(e.ClrType));

            foreach (var entityType in userOwnedEntityTypes)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
            }
        }

        /// <summary>
        /// Динамически конструирует дерево выражений (Expression Tree) для глобальной фильтрации данных.
        /// Генерирует строго типизированную лямбду вида: e => e.UserId == fakeContext.CurrentUserId
        /// </summary>
        /// <param name="entityType">Системный тип данных (Type) конкретного C#-класса сущности (например, typeof(Complaint) или typeof(Residence))</param>
        /// <returns></returns>
        private static LambdaExpression ConvertFilterExpression(Type entityType)
        {
            // 1. Создаем переменную-параметр, которая будет стоять в левой части LINQ-выражения: (e) => ...
            // Мы явно указываем её тип (например, класс entityType - это Complaint) и даем имя "e" (как в обычном коде c => c.Id).
            ParameterExpression parameter = Expression.Parameter(entityType, "e");

            // 2. Строим дерево доступа к свойству таблицы. Берем наш параметр "e" и обращаемся к его свойству "UserId".
            // Использование nameof(IUserOwned.UserId) страхует от опечаток. В итоге получаем структуру, аналогичную коду: e.UserId
            MemberExpression left = Expression.Property(parameter, nameof(IUserOwned.UserId));

            // 3. Объявляем пустую (null) переменную нашего контекста базы данных.
            // Физически она нам здесь не нужна, но она жизненно необходима компилятору C# как "зацепка" для типов в следующем шаге.
            ApplicationDbContext? fakeContext = null;

            // 4. Строим дерево доступа к правой части сравнения. Это ключевой шаг для обхода кэша EF Core!
            // Оборачиваем нашу null-переменную в константу типа ApplicationDbContext и говорим дереву: "Прочитай свойство CurrentUserId".
            // Когда EF Core увидит, что фильтр зависит от свойства класса DbContext, он сгенерирует динамический SQL-параметр (@__ef_filter_CurrentUserId_0),
            // и при каждом HTTP-запросе автоматически подставит вместо fakeContext реальный, живой экземпляр контекста.
            // Такой подход избавляет от проблемы, когда зашли в пользователь А, вышли из него, потом зашли в пользователь Б
            // и прогрузились данные от пользователя А
            MemberExpression right = Expression.Property(
                Expression.Constant(fakeContext, typeof(ApplicationDbContext)),
                nameof(ApplicationDbContext.CurrentUserId)
            );

            // 5. Объединяем левую часть (e.UserId) и правую часть (fakeContext.CurrentUserId) знаком логического равенства.
            // В оперативной памяти создается объект бинарного выражения, представляющий математическую формулу: e.UserId == fakeContext.CurrentUserId
            BinaryExpression equality = Expression.Equal(left, right);

            // 6. Собираем всё воедино и превращаем наше математическое равенство в готовую лямбда-функцию (LambdaExpression).
            // Мы передаем само условие равенства и указываем, что входным параметром для него является "e" (из Шага 1).
            // На выходе получаем структуру, идентичную LINQ-коду: e => e.UserId == fakeContext.CurrentUserId
            return Expression.Lambda(equality, parameter);
        }
    }
}
