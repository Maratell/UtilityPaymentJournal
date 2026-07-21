using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Residences.Commands;
using UtilityPaymentJournal.Features.Residences.Models;
using UtilityPaymentJournal.Features.Residences.Queries;


namespace UtilityPaymentJournal.Features.Residences
{
    /// <summary>
    /// АПИ-контроллер для управления показаниями счетчиков электроэнергии.
    /// </summary>
    [ApiController]
    [Route("api/residences")]
    public class ResidencesApiController : ControllerBase
    {
        private readonly IResidenceQueryService _queryService;
        private readonly IResidenceCommandService _commandService;
        private readonly IResidenceMapper _residenceMapper;

        public ResidencesApiController(
            IResidenceQueryService queryService,
            IResidenceCommandService commandService,
            IResidenceMapper residenceMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ResidenceDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ResidenceQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);
            ResidenceDetailsViewModel[] viewModels = dtos
                .Select(r => _residenceMapper.ToViewModel(r))
                .ToArray();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получить развернутые детали показания счетчика электроэнергии по его уникальному идентификатору.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ResidenceDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ResidenceQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            ResidenceDetailsViewModel viewModel = _residenceMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создать новую запись показания счетчика электроэнергии.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ResidenceCreatedViewModel>> Create([FromBody] CreateResidenceViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateResidenceDto createDto = _residenceMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ResidenceCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            ResidenceCreatedViewModel resultViewModel = _residenceMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }

        /// <summary>
        /// Отредактировать существующие данные показания счетчика электроэнергии.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ResidenceUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditResidenceViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditResidenceDto editDto = _residenceMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ResidenceCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            ResidenceUpdatedViewModel resultViewModel = _residenceMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }

        /// <summary>
        /// Удалить запись показания счетчика электроэнергии из системы.
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
