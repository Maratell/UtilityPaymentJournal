using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/utility-providers")]
    public class UtilityProvidersApiController : ControllerBase
    {
        private readonly IUtilityProviderService _utilityProviderService;
        private readonly IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProvidersApiController(
            IUtilityProviderService utilityProviderService,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _utilityProviderService = utilityProviderService;
            _utilityProviderMapper = utilityProviderMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityProviderViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<UtilityProviderDTO> dtos = await _utilityProviderService.GetAllAsync(cancellationToken);

            List<UtilityProviderViewModel> viewModels = dtos
                .Select(dto => _utilityProviderMapper.ToViewModel(dto))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityProviderViewModel>> GetById(long id, CancellationToken cancellationToken)
        {
            UtilityProviderDTO? dto = await _utilityProviderService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Поставщик услуг с ID {id} не найден.");
            }

            UtilityProviderViewModel viewModel = _utilityProviderMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<UtilityProviderViewModel>> Create([FromBody] CreateUtilityProviderViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateUtilityProviderDTO createDto = _utilityProviderMapper.ToDto(createViewModel);

            UtilityProviderDTO createdDto = await _utilityProviderService.CreateAsync(createDto, cancellationToken);

            UtilityProviderViewModel createdViewModel = _utilityProviderMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<UtilityProviderViewModel>> Edit(long id, [FromBody] EditUtilityProviderViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditUtilityProviderDTO editDto = _utilityProviderMapper.ToDto(editViewModel);

            UtilityProviderDTO? updatedDto = await _utilityProviderService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Поставщик услуг с ID {id} не найден.");
            }

            UtilityProviderViewModel updatedViewModel = _utilityProviderMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _utilityProviderService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Поставщик услуг с ID {id} не найден.");
            }

            return NoContent();
        }
    }
}
