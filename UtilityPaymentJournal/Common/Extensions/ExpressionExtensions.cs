using System.Linq.Expressions;

namespace UtilityPaymentJournal.Common.Extensions
{
    /// <summary>
    /// Предоставляет методы расширения для работы с деревьями выражений (Expression Trees),
    /// позволяющие динамически конструировать сложные предикаты фильтрации для Entity Framework Core.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Динамически объединяет два логических выражения (предиката) по правилу "И" (AND).
        /// </summary>
        /// <typeparam name="T">Тип доменной сущности, к которой применяется фильтр</typeparam>
        /// <param name="expr1">Исходное базовое выражение фильтрации</param>
        /// <param name="expr2">Дополнительное выражение фильтрации, которое необходимо приклеить к первому</param>
        /// <returns>Новое объединенное дерево выражений, представляющее собой логическую связку (expr1 && expr2)</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // Предотвращаем падение, если на вход случайно передали пустые предикаты
            ArgumentNullException.ThrowIfNull(expr1);
            ArgumentNullException.ThrowIfNull(expr2);

            // Инкапсулируем параметры второго выражения, подстраивая их под контекст первого выражения
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            // Формируем результирующее лямбда-выражение, соединяя тела двух предикатов бинарным оператором AndAlso (логическое &&)
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
