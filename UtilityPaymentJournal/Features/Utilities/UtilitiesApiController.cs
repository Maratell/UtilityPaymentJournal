using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Features.Utilities.Commands;
using UtilityPaymentJournal.Features.Utilities.Models;
using UtilityPaymentJournal.Features.Utilities.Queries;

namespace UtilityPaymentJournal.Features.Utilities
{
    /// <summary>
    /// АПИ-контроллер для управления коммунальными услугами.
    /// </summary>
    [ApiController]
    [Route("api/utilities")]
    public class UtilitiesApiController : ControllerBase
    {
        private readonly IUtilityQueryService _queryService;
        private readonly IUtilityCommandService _commandService;
        private readonly IUtilityMapper _utilityMapper;

        public UtilitiesApiController(
            IUtilityQueryService queryService,
            IUtilityCommandService commandService,
            IUtilityMapper utilityMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _utilityMapper = utilityMapper ?? throw new ArgumentNullException(nameof(utilityMapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityDetailsViewModel>>> GetAll([FromQuery] UtilityQueryFilter filter, CancellationToken cancellationToken)
        {
            // Создаем спецификацию на основе параметров, которые прислал UI 
            ICriteriaSpecification<Utility> criteria = new UtilityFilterSpecification(filter);

            IReadOnlyCollection<UtilityQueryResultDto> dtos = await _queryService.GetAllAsync(criteria, cancellationToken);
            UtilityDetailsViewModel[] viewModels = dtos.Select(_utilityMapper.ToViewModel).ToArray();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            UtilityDetailsViewModel viewModel = _utilityMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<UtilityCreatedViewModel>> Create([FromBody] CreateUtilityViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateUtilityDto createDto = _utilityMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            UtilityCreatedViewModel resultViewModel = _utilityMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<UtilityUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditUtilityViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditUtilityDto editDto = _utilityMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            UtilityUpdatedViewModel resultViewModel = _utilityMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _commandService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
