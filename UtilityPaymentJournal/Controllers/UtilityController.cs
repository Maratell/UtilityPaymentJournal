using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Controllers 
{
    public class UtilityController : Controller
    {
        private IUtilityService _utilityService;
        private IUtilityMapper _utilityMapper;

        public UtilityController(
            IUtilityService utilityService,
            IUtilityMapper utilityMapper)
        {
            _utilityService = utilityService;
            _utilityMapper = utilityMapper;
        }

        public IActionResult GetView()
        {
            return View("~/Views/Utility/Utility.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUtilityViewModel createUtilityViewModel)
        {
            CreateUtilityDTO utilityDto = _utilityMapper.ToDto(createUtilityViewModel);

            UtilityDTO result = await _utilityService.CreateAsync(utilityDto);

            UtilityViewModel utilityViewModel = _utilityMapper.ToViewModel(result);

            return Json(utilityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UtilityDTO> result = await _utilityService.GetAllAsync();

            IEnumerable<UtilityViewModel> utilitys = result.Select(r => _utilityMapper.ToViewModel(r));

            return Json(utilitys);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(long id, EditUtilityViewModel editUtilityViewModel)
        {
            EditUtilityDTO editUtilityDto = _utilityMapper.ToDto(editUtilityViewModel);

            UtilityDTO result = await _utilityService.EditAsync(id, editUtilityDto);

            UtilityViewModel utilityViewModel = _utilityMapper.ToViewModel(result);

            return Json(utilityViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _utilityService.DeleteAsync(id);

            return Ok();
        }
    }
}
