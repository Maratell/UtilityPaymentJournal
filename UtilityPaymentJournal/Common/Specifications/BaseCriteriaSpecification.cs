using System.Linq.Expressions;

namespace UtilityPaymentJournal.Common.Specifications
{
    /// <summary>
    /// Базовая абстрактная реализация спецификации критериев фильтрации.
    /// Используется как основа для создания конкретных независимых классов-фильтров для SQL-запросов.
    /// </summary>
    /// <typeparam name="T">Тип доменной сущности, к которой применяется фильтр</typeparam>
    public abstract class BaseCriteriaSpecification<T> : ICriteriaSpecification<T>
    {
        /// <summary>
        /// Критерий (правило) фильтрации в виде дерева выражений (Expression Tree)
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Инициализирует новый экземпляр спецификации с жестко заданным критерием отбора.
        /// </summary>
        /// <param name="criteria">Лямбда-выражение бизнес-правила (например, u => u.IsActive == true), возвращающее bool</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданное выражение критерия равно null</exception>
        protected BaseCriteriaSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }
    }
}
