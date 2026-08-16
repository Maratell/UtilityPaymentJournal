using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// Спецификация, пропускающая абсолютно все записи коммунальных услуг.
    /// </summary>
    public class AllUtilitiesSpecification : BaseCriteriaSpecification<Utility>
    {
        public AllUtilitiesSpecification()
            : base(_ => true) // Условие всегда true, выберет всю таблицу
        { }
    }
}
