using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers
{
    public class UtilityProviderController : Controller
    {
        private IUtilityProviderService _utilityProviderService;
        private IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProviderController(
            IUtilityProviderService utilityProviderService,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _utilityProviderService = utilityProviderService;
            _utilityProviderMapper = utilityProviderMapper;
        }

        public IActionResult GetView()
        {
            return View("~/Views/Utility/Utility.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUtilityProviderViewModel createUtilityProviderViewModel)
        {
            CreateUtilityProviderDTO utilityProviderDto = _utilityProviderMapper.ToDto(createUtilityProviderViewModel);

            UtilityProviderDTO result = await _utilityProviderService.CreateAsync(utilityProviderDto);

            UtilityProviderViewModel utilityProviderViewModel = _utilityProviderMapper.ToViewModel(result);

            return Json(utilityProviderViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UtilityProviderDTO> result = await _utilityProviderService.GetAllAsync();

            IEnumerable<UtilityProviderViewModel> utilityProviders = result.Select(r => _utilityProviderMapper.ToViewModel(r));

            return Json(utilityProviders);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(long id, EditUtilityProviderViewModel editUtilityProviderViewModel)
        {
            EditUtilityProviderDTO editUtilityDto = _utilityProviderMapper.ToDto(editUtilityProviderViewModel);

            UtilityProviderDTO result = await _utilityProviderService.EditAsync(id, editUtilityDto);

            UtilityProviderViewModel utilityProviderViewModel = _utilityProviderMapper.ToViewModel(result);

            return Json(utilityProviderViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _utilityProviderService.DeleteAsync(id);

            return Ok();
        }
    }
}
