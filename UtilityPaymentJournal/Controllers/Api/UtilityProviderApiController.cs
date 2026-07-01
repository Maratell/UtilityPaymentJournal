using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/utility-providers")]
    public class UtilityProviderApiController : ControllerBase
    {
        private readonly IUtilityProviderService _utilityProviderService;
        private readonly IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProviderApiController(
            IUtilityProviderService utilityProviderService,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _utilityProviderService = utilityProviderService;
            _utilityProviderMapper = utilityProviderMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilityProviderViewModel>>> GetAll()
        {
            IEnumerable<UtilityProviderDTO> result = await _utilityProviderService.GetAllAsync();

            IEnumerable<UtilityProviderViewModel> viewModels = result.Select(r => _utilityProviderMapper.ToViewModel(r));

            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<ActionResult<UtilityProviderViewModel>> Create([FromBody] CreateUtilityProviderViewModel createUtilityProviderViewModel)
        {
            CreateUtilityProviderDTO dto = _utilityProviderMapper.ToDto(createUtilityProviderViewModel);

            UtilityProviderDTO result = await _utilityProviderService.CreateAsync(dto);

            UtilityProviderViewModel createdViewModel = _utilityProviderMapper.ToViewModel(result);

            return CreatedAtAction(nameof(GetAll), createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<UtilityProviderViewModel>> Edit(long id, [FromBody] EditUtilityProviderViewModel editUtilityProviderViewModel)
        {
            EditUtilityProviderDTO dto = _utilityProviderMapper.ToDto(editUtilityProviderViewModel);

            UtilityProviderDTO result = await _utilityProviderService.EditAsync(id, dto);

            UtilityProviderViewModel updatedViewModel = _utilityProviderMapper.ToViewModel(result);

            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _utilityProviderService.DeleteAsync(id);

            return NoContent();
        }
    }
}
