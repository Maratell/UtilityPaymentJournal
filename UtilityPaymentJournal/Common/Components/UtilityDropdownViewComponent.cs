using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Common.Components
{
    public class UtilityDropdownViewComponent : ViewComponent
    {
        private readonly IUtilityService _utilityService;
        private readonly IUtilityMapper _utilityMapper;

        public UtilityDropdownViewComponent(IUtilityService utilityService, IUtilityMapper utilityMapper)
        {
            _utilityService = utilityService;
            _utilityMapper = utilityMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IReadOnlyCollection<UtilityDto> dtos = await _utilityService
                .GetAllAsync();

            UtilityViewModel[] viewModels = dtos.Select(_utilityMapper.ToViewModel)
                .ToArray();

            return View(viewModels);
        }
    }
}
