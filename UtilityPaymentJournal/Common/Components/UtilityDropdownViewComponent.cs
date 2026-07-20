using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Features.Utilities;
using UtilityPaymentJournal.Features.Utilities.Models;
using UtilityPaymentJournal.Features.Utilities.Queries;

namespace UtilityPaymentJournal.Common.Components
{
    public class UtilityDropdownViewComponent : ViewComponent
    {
        private readonly IUtilityQueryService _utilityQueryService;
        private readonly IUtilityMapper _utilityMapper;

        public UtilityDropdownViewComponent(IUtilityQueryService utilityQueryService, IUtilityMapper utilityMapper)
        {
            _utilityQueryService = utilityQueryService;
            _utilityMapper = utilityMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            // отображаем для выбора только активные коммунальные услуги
            UtilityQueryFilter filter = new UtilityQueryFilter(IsActive: true);
            ICriteriaSpecification<Utility> criteria = new UtilityFilterSpecification(filter);

            IReadOnlyCollection<UtilityQueryResultDto> dtos = await _utilityQueryService
                .GetAllAsync(criteria, cancellationToken);

            UtilityDetailsViewModel[] viewModels = dtos.Select(_utilityMapper.ToViewModel)
                .ToArray();

            return View(viewModels);
        }
    }
}
