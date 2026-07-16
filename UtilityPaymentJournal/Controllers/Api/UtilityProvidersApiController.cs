using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/utility-providers")]
    public partial class UtilityProvidersApiController : ControllerBase
    {
        private readonly IUtilityProviderService _utilityProviderService;
        private readonly IUtilityProviderMapper _utilityProviderMapper;
        private readonly ILogger<UtilityProvidersApiController> _logger;

        public UtilityProvidersApiController(
            IUtilityProviderService utilityProviderService,
            IUtilityProviderMapper utilityProviderMapper,
            ILogger<UtilityProvidersApiController> logger)
        {
            _utilityProviderService = utilityProviderService ?? throw new ArgumentNullException(nameof(utilityProviderService));
            _utilityProviderMapper = utilityProviderMapper ?? throw new ArgumentNullException(nameof(utilityProviderMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityProviderViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllUtilityProviders(_logger);

            IEnumerable<UtilityProviderDto> dtos = await _utilityProviderService.GetAllAsync(cancellationToken);
            UtilityProviderViewModel[] viewModels = dtos
                .Select(dto => _utilityProviderMapper.ToViewModel(dto))
                .ToArray();

            LogFetchedAllUtilityProvidersCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityProviderViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityProviderDto dto = await _utilityProviderService.GetByIdAsync(id, cancellationToken);

            UtilityProviderViewModel viewModel = _utilityProviderMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<UtilityProviderViewModel>> Create([FromBody] CreateUtilityProviderViewModel createViewModel, CancellationToken cancellationToken)
        {
            LogUtilityProviderCreationRequested(_logger, createViewModel.Name);

            CreateUtilityProviderDto createDto = _utilityProviderMapper.ToDto(createViewModel);
            UtilityProviderDto createdDto = await _utilityProviderService.CreateAsync(createDto, cancellationToken);
            UtilityProviderViewModel createdViewModel = _utilityProviderMapper.ToViewModel(createdDto);

            LogUtilityProviderCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<UtilityProviderViewModel>> Edit([FromRoute] long id, [FromBody] EditUtilityProviderViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogUtilityProviderUpdateRequested(_logger, id, editViewModel.Name);

            EditUtilityProviderDto editDto = _utilityProviderMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            UtilityProviderDto updatedDto = await _utilityProviderService.EditAsync(id, editDto, cancellationToken);
            UtilityProviderViewModel updatedViewModel = _utilityProviderMapper.ToViewModel(updatedDto);

            LogUtilityProviderUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogUtilityProviderDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _utilityProviderService.DeleteAsync(id, cancellationToken);

            LogUtilityProviderDeleted(_logger, id);
            return NoContent();
        }
    }
}
