using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/utilities")]
    public partial class UtilitiesApiController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IUtilityMapper _utilityMapper;
        private readonly ILogger<UtilitiesApiController> _logger;

        public UtilitiesApiController(
            IUtilityService utilityService,
            IUtilityMapper utilityMapper,
            ILogger<UtilitiesApiController> logger)
        {
            _utilityService = utilityService ?? throw new ArgumentNullException(nameof(utilityService));
            _utilityMapper = utilityMapper ?? throw new ArgumentNullException(nameof(utilityMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllUtilities(_logger);

            IEnumerable<UtilityDto> dtos = await _utilityService.GetAllAsync(cancellationToken);
            UtilityViewModel[] viewModels = dtos
                .Select(dto => _utilityMapper.ToViewModel(dto))
                .ToArray();

            LogFetchedAllUtilitiesCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityDto dto = await _utilityService.GetByIdAsync(id, cancellationToken);

            UtilityViewModel viewModel = _utilityMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilityViewModel createViewModel, CancellationToken cancellationToken)
        {
            LogUtilityCreationRequested(_logger, createViewModel.Name);

            CreateUtilityDto createDto = _utilityMapper.ToDto(createViewModel);
            UtilityDto createdDto = await _utilityService.CreateAsync(createDto, cancellationToken);
            UtilityViewModel createdViewModel = _utilityMapper.ToViewModel(createdDto);

            LogUtilityCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }



        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EditUtilityViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogUtilityUpdateRequested(_logger, id, editViewModel.Name);

            EditUtilityDto editDto = _utilityMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityDto updatedDto = await _utilityService.EditAsync(id, editDto, cancellationToken);
            UtilityViewModel updatedViewModel = _utilityMapper.ToViewModel(updatedDto);

            LogUtilityUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogUtilityDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _utilityService.DeleteAsync(id, cancellationToken);


            LogUtilityDeleted(_logger, id);
            return NoContent();
        }
    }
}
