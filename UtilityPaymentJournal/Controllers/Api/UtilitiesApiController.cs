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
            _utilityService = utilityService;
            _utilityMapper = utilityMapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<UtilityDto> dtos = await _utilityService.GetAllAsync(cancellationToken);

            IEnumerable<UtilityViewModel> viewModels = dtos
                .Select(dto => _utilityMapper.ToViewModel(dto))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            UtilityDto? dto = await _utilityService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Услуга с ID {id} не найдена.");
            }

            UtilityViewModel viewModel = _utilityMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilityViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateUtilityDto createDto = _utilityMapper.ToDto(createViewModel);

            UtilityDto createdDto = await _utilityService.CreateAsync(createDto, cancellationToken);

            UtilityViewModel createdViewModel = _utilityMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }



        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EditUtilityViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditUtilityDto editDto = _utilityMapper.ToDto(editViewModel);

            UtilityDto? updatedDto = await _utilityService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Услуга с ID {id} не найдена.");
            }

            UtilityViewModel updatedViewModel = _utilityMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _utilityService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Услуга с ID {id} не найдена.");
            }

            LogUtilityDeleted(_logger, id);
            return NoContent();
        }

        #region Logger Messages

        [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Услуга {id} успешно удалена из системы")]
        private static partial void LogUtilityDeleted(ILogger logger, long id);

        #endregion
    }
}
