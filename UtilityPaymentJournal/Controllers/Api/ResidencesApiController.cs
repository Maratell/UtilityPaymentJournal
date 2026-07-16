using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Residences;


namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/residences")]
    public partial class ResidencesApiController : ControllerBase
    {
        private readonly IResidenceService _residenceService;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidencesApiController> _logger;

        public ResidencesApiController(
            IResidenceService residenceService,
            IResidenceMapper residenceMapper,
            ILogger<ResidencesApiController> logger)
        {
            _residenceService = residenceService ?? throw new ArgumentNullException(nameof(residenceService));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ResidenceViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllResidences(_logger);

            IEnumerable<ResidenceDto> dtos = await _residenceService.GetAllAsync(cancellationToken);
            ResidenceViewModel[] viewModels = dtos
                .Select(_residenceMapper.ToViewModel)
                .ToArray();

            LogFetchedAllResidencesCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ResidenceDto dto = await _residenceService.GetByIdAsync(id, cancellationToken);

            ResidenceViewModel viewModel = _residenceMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ResidenceViewModel>> Create([FromBody] CreateResidenceViewModel createResidence, CancellationToken cancellationToken)
        {
            LogResidenceCreationRequested(_logger, createResidence.Address);

            CreateResidenceDto createDto = _residenceMapper.ToDto(createResidence);
            ResidenceDto createdDto = await _residenceService.CreateAsync(createDto, cancellationToken);
            ResidenceViewModel createdViewModel = _residenceMapper.ToViewModel(createdDto);

            LogResidenceCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> Edit([FromRoute] long id, [FromBody] EditResidenceViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogResidenceUpdateRequested(_logger, id, editViewModel.Address);

            EditResidenceDto editDto = _residenceMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ResidenceDto updatedDto = await _residenceService.EditAsync(id, editDto, cancellationToken);

            ResidenceViewModel updatedViewModel = _residenceMapper.ToViewModel(updatedDto);
            LogResidenceUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogResidenceDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _residenceService.DeleteAsync(id, cancellationToken);

            LogResidenceDeleted(_logger, id);
            return NoContent();
        }
    }
}
