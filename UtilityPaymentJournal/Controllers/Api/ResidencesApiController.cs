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
        public async Task<ActionResult<IReadOnlyCollection<ResidenceViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ResidenceDTO> dtos = await _residenceService.GetAllAsync(cancellationToken);

            List<ResidenceViewModel> viewModels = dtos
                .Select(r => _residenceMapper.ToViewModel(r))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            ResidenceDTO? dto = await _residenceService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Жилой объект с ID {id} не найден.");
            }

            ResidenceViewModel viewModel = _residenceMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ResidenceViewModel>> Create([FromBody] CreateResidenceViewModel createResidence, CancellationToken cancellationToken)
        {
            CreateResidenceDTO createDto = _residenceMapper.ToDto(createResidence);

            ResidenceDTO createdDto = await _residenceService.CreateAsync(createDto, cancellationToken);

            ResidenceViewModel createdViewModel = _residenceMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> Edit([FromRoute] long id, [FromBody] EditResidenceViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditResidenceDTO editDto = _residenceMapper.ToDto(editViewModel);

            ResidenceDTO? updatedDto = await _residenceService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Жилой объект с ID {id} не найден.");
            }

            ResidenceViewModel updatedViewModel = _residenceMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _residenceService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Жилой объект с ID {id} не найден.");
            }

            return NoContent();
        }
    }
}
