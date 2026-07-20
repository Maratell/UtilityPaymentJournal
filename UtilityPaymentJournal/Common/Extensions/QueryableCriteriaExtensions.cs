using UtilityPaymentJournal.Common.Specifications;

namespace UtilityPaymentJournal.Common.Extensions
{
    /// <summary>
    /// Предоставляет методы расширения для интерфейса <see cref="IQueryable{T}"/>, 
    /// позволяющие декларативно применять объектно-ориентированные спецификации критериев к запросам Entity Framework Core.
    /// </summary>
    public static class QueryableCriteriaExtensions
    {
        /// <summary>
        /// Декларативно накладывает на текущий LINQ-запрос к БД спецификацию критериев фильтрации.
        /// </summary>
        /// <typeparam name="T">Тип обрабатываемой entity сущности</typeparam>
        /// <param name="inputQuery">Исходный конвейер запроса (IQueryable), идущий к таблице БД</param>
        /// <param name="criteriaSpecification">Спецификация, содержащая изолированное бизнес-правило отбора данных</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если исходный запрос или спецификация критериев равны null</exception>
        public static IQueryable<T> FilterWith<T>(this IQueryable<T> inputQuery, ICriteriaSpecification<T> criteriaSpecification) where T : class
        {
            ArgumentNullException.ThrowIfNull(inputQuery);
            ArgumentNullException.ThrowIfNull(criteriaSpecification);

            // Извлекаем из спецификации дерево выражений (Criteria) и передаем его в стандартный метод .Where() от EF Core.
            // Именно в этот момент абстрактное бизнес-правило превращается в понятный для ORM LINQ-фильтр,
            // который при трансляции в SQL станет оператором "WHERE [Поле] = [Значение]".
            return inputQuery.Where(criteriaSpecification.Criteria);
        }
    }
}
