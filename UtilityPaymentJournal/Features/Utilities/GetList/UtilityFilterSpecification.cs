using System.Linq.Expressions;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    /// <summary>
    /// Динамическая спецификация критериев фильтрации коммунальных услуг, строящая SQL-условие WHERE на основе параметров из UI.
    /// </summary>
    public class UtilityFilterSpecification : BaseCriteriaSpecification<Utility>
    {
        /// <summary>
        /// Инициализирует спецификацию, генерируя динамическое дерево выражений на основе прилетевшего фильтра.
        /// </summary>
        /// <param name="filter">Объект параметров фильтрации, прилетевший из UI/AJAX</param>
        public UtilityFilterSpecification(UtilityQueryFilter filter) 
            : base(BuildCriteria(filter))
        { }

        private static Expression<Func<Utility, bool>> BuildCriteria(UtilityQueryFilter filter)
        {
            // 1. Инициализируем базовое выражение. Если флаг передан — берем его, иначе — заглушку "выбрать всё"
            Expression<Func<Utility, bool>> criteria = filter.IsActive.HasValue
                ? u => u.IsActive == filter.IsActive.Value
                : u => true;

            // 2. Если есть поисковое слово — доклеиваем его через метод расширения .And()
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                criteria = criteria.And(u => u.Name.Contains(filter.SearchTerm));
            }

            return criteria;
        }
    }
}
