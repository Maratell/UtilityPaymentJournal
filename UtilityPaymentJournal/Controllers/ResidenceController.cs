using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Residences;


namespace UtilityPaymentJournal.Controllers
{
    public class ResidenceController : Controller
    {
        private IResidenceService _residenceService;
        private IResidenceMapper _residenceMapper;

        public ResidenceController(
            IResidenceService residenceService,
            IResidenceMapper residenceMapper)
        {
            _residenceService = residenceService;
            _residenceMapper = residenceMapper;
        }

        public IActionResult GetView()
        {
            return View("Residence");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateResidenceViewModel createResidence)
        {
            CreateResidenceDTO residenceDTO = _residenceMapper.ToDto(createResidence);

            ResidenceDTO result = await _residenceService.CreateAsync(residenceDTO);

            ResidenceViewModel residenceVM = _residenceMapper.ToViewModel(result);

            return Json(residenceVM);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ResidenceDTO> result = await _residenceService.GetAllAsync();

            IEnumerable<ResidenceViewModel> residences = result.Select(r => _residenceMapper.ToViewModel(r));

            return Json(residences);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(long id, EditResidenceViewModel editResidenceVm)
        {
            EditResidenceDTO editResidenceDto = _residenceMapper.ToDto(editResidenceVm);

            ResidenceDTO result = await _residenceService.EditAsync(id, editResidenceDto);

            ResidenceViewModel residenceVm = _residenceMapper.ToViewModel(result); 
            
            return Json(residenceVm); 
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _residenceService.DeleteAsync(id);

            return Ok();
        }
    }
}
