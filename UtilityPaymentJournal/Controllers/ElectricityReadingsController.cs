using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Controllers
{
    public class ElectricityReadingsController : Controller
    {
        private IElectricityReadingService _electricityReadingService;
        private IElectricityReadingMapper _electricityReadingMapper;

        public ElectricityReadingsController(
            IElectricityReadingService electricityReadingService,
            IElectricityReadingMapper electricityReadingMapper)
        {
            _electricityReadingService = electricityReadingService;
            _electricityReadingMapper = electricityReadingMapper;
        }

        public IActionResult GetView()
        {
            return View("~/Views/Tables/ElectricityReadingsTable.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateElectricityReadingViewModel createElectricityReadingViewModel)
        {
            CreateElectricityReadingDTO residenceDTO = _electricityReadingMapper.ToDto(createElectricityReadingViewModel);

            ElectricityReadingDTO result = await _electricityReadingService.CreateAsync(residenceDTO);

            ElectricityReadingViewModel electricityReadingViewModel = _electricityReadingMapper.ToViewModel(result);

            return Json(new { success = true, data = electricityReadingViewModel });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, EditElectricityReadingViewModel editResidenceVm)
        {
            EditElectricityReadingDTO editResidenceDto = _electricityReadingMapper.ToDto(editResidenceVm);

            ElectricityReadingDTO result = await _electricityReadingService.EditAsync(id, editResidenceDto);

            ElectricityReadingViewModel electricityReadingViewModel = _electricityReadingMapper.ToViewModel(result);

            return Json(new { success = true, data = electricityReadingViewModel });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ElectricityReadingDTO> result = await _electricityReadingService.GetAllAsync();

            IEnumerable<ElectricityReadingViewModel> electricityReadings = result.Select(r => _electricityReadingMapper.ToViewModel(r));

            return Json(electricityReadings);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _electricityReadingService.DeleteAsync(id);

            return Ok(new { success = true, message = "Запись успешно удалена" });
        }
    }
}
