using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.WaterReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.WaterReadings;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/water-readings")]
    public partial class WaterReadingsApiController : ControllerBase
    {
        private readonly IWaterReadingService _waterReadingService;
        private readonly IWaterReadingMapper _waterReadingMapper;
        private readonly ILogger<WaterReadingsApiController> _logger;

        public WaterReadingsApiController(
            IWaterReadingService waterReadingService,
            IWaterReadingMapper waterReadingMapper,
            ILogger<WaterReadingsApiController> logger)
        {
            _waterReadingService = waterReadingService ?? throw new ArgumentNullException(nameof(waterReadingService));
            _waterReadingMapper = waterReadingMapper ?? throw new ArgumentNullException(nameof(waterReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<WaterReadingViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllWaterReadings(_logger);

            IEnumerable<WaterReadingDto> dtos = await _waterReadingService.GetAllAsync(cancellationToken);
            WaterReadingViewModel[] viewModels = dtos
                .Select(dto => _waterReadingMapper.ToViewModel(dto))
                .ToArray();

            LogFetchedAllWaterReadingsCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<WaterReadingViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            WaterReadingDto dto = await _waterReadingService.GetByIdAsync(id, cancellationToken);
            WaterReadingViewModel viewModel = _waterReadingMapper.ToViewModel(dto);

            return Ok(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult<WaterReadingViewModel>> Create([FromBody] CreateWaterReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            LogWaterReadingCreationRequested(_logger, createViewModel.CurrentValue);

            CreateWaterReadingDto createDto = _waterReadingMapper.ToDto(createViewModel);
            WaterReadingDto createdDto = await _waterReadingService.CreateAsync(createDto, cancellationToken);
            WaterReadingViewModel createdViewModel = _waterReadingMapper.ToViewModel(createdDto);

            LogWaterReadingCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<WaterReadingViewModel>> Edit([FromRoute] long id, [FromBody] EditWaterReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogWaterReadingUpdateRequested(_logger, id, editViewModel.CurrentValue);

            EditWaterReadingDto editDto = _waterReadingMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            WaterReadingDto updatedDto = await _waterReadingService.EditAsync(id, editDto, cancellationToken);
            WaterReadingViewModel updatedViewModel = _waterReadingMapper.ToViewModel(updatedDto);

            LogUtilityProviderUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogWaterReadingDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _waterReadingService.DeleteAsync(id, cancellationToken);

            LogWaterReadingDeleted(_logger, id);
            return NoContent();
        }
    }
}
