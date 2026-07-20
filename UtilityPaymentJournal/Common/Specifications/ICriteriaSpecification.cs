using System.Linq.Expressions;

namespace UtilityPaymentJournal.Common.Specifications
{
    /// <summary>
    /// Интерфейс для построения изолированных бизнес-спецификаций фильтрации.
    /// Отвечает исключительно за инкапсуляцию критериев отбора данных (SQL-условие WHERE).
    /// </summary>
    /// <typeparam name="T">Тип entity (например, Utility, Residence), к которой применяются критерии</typeparam>
    public interface ICriteriaSpecification<T>
    {
        /// <summary>
        /// Критерий (правило) фильтрации в виде дерева выражений (Expression Tree).
        /// Представляет собой предикат (лямбду, возвращающую bool), который Entity Framework Core 
        /// способен проанализировать и транслировать в SQL-оператор WHERE на стороне базы данных.
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }
    }
}
