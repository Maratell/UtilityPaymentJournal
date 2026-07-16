using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/electricity-readings")]
    public partial class ElectricityReadingsApiController : ControllerBase
    {
        private readonly IElectricityReadingService _electricityReadingService;
        private readonly IElectricityReadingMapper _electricityReadingMapper;
        private readonly ILogger<ElectricityReadingsApiController> _logger;

        public ElectricityReadingsApiController(
            IElectricityReadingService electricityReadingService,
            IElectricityReadingMapper electricityReadingMapper,
            ILogger<ElectricityReadingsApiController> logger)
        {
            _electricityReadingService = electricityReadingService ?? throw new ArgumentNullException(nameof(electricityReadingService));
            _electricityReadingMapper = electricityReadingMapper ?? throw new ArgumentNullException(nameof(electricityReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ElectricityReadingViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllElectricityReadings(_logger);

            IEnumerable<ElectricityReadingDto> dtos = await _electricityReadingService.GetAllAsync(cancellationToken);
            ElectricityReadingViewModel[] viewModels = dtos
                .Select(r => _electricityReadingMapper.ToViewModel(r))
                .ToArray();

            LogFetchedAllElectricityReadingsCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ElectricityReadingViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ElectricityReadingDto dto = await _electricityReadingService.GetByIdAsync(id, cancellationToken);
            ElectricityReadingViewModel viewModel = _electricityReadingMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ElectricityReadingViewModel>> Create([FromBody] CreateElectricityReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            LogElectricityReadingCreationRequested(_logger, createViewModel.CurrentValue);

            CreateElectricityReadingDto createDto = _electricityReadingMapper.ToDto(createViewModel);
            ElectricityReadingDto createdDto = await _electricityReadingService.CreateAsync(createDto, cancellationToken);
            ElectricityReadingViewModel createdViewModel = _electricityReadingMapper.ToViewModel(createdDto);

            LogElectricityReadingCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ElectricityReadingViewModel>> Edit([FromRoute] long id, [FromBody] EditElectricityReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogElectricityReadingUpdateRequested(_logger, id, editViewModel.CurrentValue);

            EditElectricityReadingDto editDto = _electricityReadingMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ElectricityReadingDto updatedDto = await _electricityReadingService.EditAsync(id, editDto, cancellationToken);
            ElectricityReadingViewModel updatedViewModel = _electricityReadingMapper.ToViewModel(updatedDto);

            LogElectricityReadingUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogElectricityReadingDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _electricityReadingService.DeleteAsync(id, cancellationToken);

            LogElectricityReadingDeleted(_logger, id);
            return NoContent();
        }
    }
}
