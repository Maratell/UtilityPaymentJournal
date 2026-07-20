using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// Спецификация для фильтрации коммунальных услуг по их статусу активности.
    /// </summary>
    public class UtilityIsActiveSpecification : BaseCriteriaSpecification<Utility>
    {
        public UtilityIsActiveSpecification(bool isActive)
            : base(u => u.IsActive == isActive)
        { }
    }
}
