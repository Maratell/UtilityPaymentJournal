using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.UtilityProviders.Commands;
using UtilityPaymentJournal.Features.UtilityProviders.Models;
using UtilityPaymentJournal.Features.UtilityProviders.Queries;


namespace UtilityPaymentJournal.Features.UtilityProviders
{
    [ApiController]
    [Route("api/utility-providers")]
    public class UtilityProvidersApiController : ControllerBase
    {
        private readonly IUtilityProviderQueryService _queryService;
        private readonly IUtilityProviderCommandService _commandService;
        private readonly IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProvidersApiController(
            IUtilityProviderQueryService queryService,
            IUtilityProviderCommandService commandService,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _utilityProviderMapper = utilityProviderMapper ?? throw new ArgumentNullException(nameof(utilityProviderMapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityProviderDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<UtilityProviderQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);
            UtilityProviderDetailsViewModel[] viewModels = dtos
                .Select(p => _utilityProviderMapper.ToViewModel(p))
                .ToArray();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получить развернутые детали поставщика коммунальных услуг по его уникальному идентификатору.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityProviderDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityProviderQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            UtilityProviderDetailsViewModel viewModel = _utilityProviderMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создать новую запись поставщика коммунальных услуг.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UtilityProviderCreatedViewModel>> Create([FromBody] CreateUtilityProviderViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateUtilityProviderDto createDto = _utilityProviderMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityProviderCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            UtilityProviderCreatedViewModel resultViewModel = _utilityProviderMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }

        /// <summary>
        /// Отредактировать существующие данные поставщика коммунальных услуг.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<UtilityProviderUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditUtilityProviderViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditUtilityProviderDto editDto = _utilityProviderMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityProviderCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            UtilityProviderUpdatedViewModel resultViewModel = _utilityProviderMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }

        /// <summary>
        /// Удалить запись поставщика коммунальных услуг из системы.
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _commandService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
