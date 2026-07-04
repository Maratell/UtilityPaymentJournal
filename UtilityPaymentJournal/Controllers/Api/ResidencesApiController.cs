using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/residences")]
    public class ResidencesApiController : ControllerBase
    {
        private readonly IResidenceService _residenceService;
        private readonly IResidenceMapper _residenceMapper;

        public ResidencesApiController(
            IResidenceService residenceService,
            IResidenceMapper residenceMapper)
        {
            _residenceService = residenceService;
            _residenceMapper = residenceMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResidenceViewModel>>> GetAll()
        {
            IEnumerable<ResidenceDTO> result = await _residenceService.GetAllAsync();

            List<ResidenceViewModel> viewModels = result
                .Select(r => _residenceMapper.ToViewModel(r))
                .ToList();

            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<ActionResult<ResidenceViewModel>> Create([FromBody] CreateResidenceViewModel createResidence)
        {
            CreateResidenceDTO dto = _residenceMapper.ToDto(createResidence);

            ResidenceDTO result = await _residenceService.CreateAsync(dto);

            ResidenceViewModel createdViewModel = _residenceMapper.ToViewModel(result);

            return CreatedAtAction(nameof(GetAll), createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] EditResidenceViewModel editResidenceVm)
        {
            EditResidenceDTO dto = _residenceMapper.ToDto(editResidenceVm);

            ResidenceDTO result = await _residenceService.EditAsync(id, dto);

            ResidenceViewModel updatedViewModel = _residenceMapper.ToViewModel(result);

            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _residenceService.DeleteAsync(id);

            return NoContent();
        }
    }
}
