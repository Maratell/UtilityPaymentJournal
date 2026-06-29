using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Controllers
{
    public class WaterReadingsController : Controller
    {
        private IWaterReadingService _waterReadingService;
        private IWaterReadingMapper _waterReadingMapper;

        public WaterReadingsController(
            IWaterReadingService waterReadingService,
            IWaterReadingMapper waterReadingMapper)
        {
            _waterReadingService = waterReadingService;
            _waterReadingMapper = waterReadingMapper;
        }

        public IActionResult GetView()
        {
            return View("~/Views/Tables/WaterReadingsTable.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWaterReadingViewModel createWaterReadingViewModel)
        {
            CreateWaterReadingDTO residenceDTO = _waterReadingMapper.ToDto(createWaterReadingViewModel);

            WaterReadingDTO result = await _waterReadingService.CreateAsync(residenceDTO);

            WaterReadingViewModel waterReadingViewModel = _waterReadingMapper.ToViewModel(result);

            return Json(new { success = true, data = waterReadingViewModel });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, EditWaterReadingViewModel editResidenceVm)
        {
            EditWaterReadingDTO editResidenceDto = _waterReadingMapper.ToDto(editResidenceVm);

            WaterReadingDTO result = await _waterReadingService.EditAsync(id, editResidenceDto);

            WaterReadingViewModel waterReadingViewModel = _waterReadingMapper.ToViewModel(result);

            return Json(new { success = true, data = waterReadingViewModel });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<WaterReadingDTO> result = await _waterReadingService.GetAllAsync();

            IEnumerable<WaterReadingViewModel> waterReadings = result.Select(r => _waterReadingMapper.ToViewModel(r));

            return Json(waterReadings);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _waterReadingService.DeleteAsync(id);

            return Ok(new { success = true, message = "Запись успешно удалена" });
        }
    }
}
