using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.ElectricityReadings.Commands;
using UtilityPaymentJournal.Features.ElectricityReadings.Models;
using UtilityPaymentJournal.Features.ElectricityReadings.Queries;

namespace UtilityPaymentJournal.Features.ElectricityReadings
{
    /// <summary>
    /// АПИ-контроллер для управления показаниями счетчиков электроэнергии.
    /// </summary>
    [ApiController]
    [Route("api/electricity-readings")]
    public class ElectricityReadingsApiController : ControllerBase
    {
        private readonly IElectricityReadingQueryService _queryService;
        private readonly IElectricityReadingCommandService _commandService;
        private readonly IElectricityReadingMapper _electricityReadingMapper;

        public ElectricityReadingsApiController(
            IElectricityReadingQueryService queryService,
            IElectricityReadingCommandService commandService,
            IElectricityReadingMapper electricityReadingMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _electricityReadingMapper = electricityReadingMapper ?? throw new ArgumentNullException(nameof(electricityReadingMapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ElectricityReadingDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            // Используем сервис запросов (Queries) для получения полного списка со всеми Include
            IReadOnlyCollection<ElectricityReadingQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);
            ElectricityReadingDetailsViewModel[] viewModels = dtos
                .Select(r => _electricityReadingMapper.ToViewModel(r))
                .ToArray();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получить развернутые детали показания счетчика электроэнергии по его уникальному идентификатору.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ElectricityReadingDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ElectricityReadingQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            ElectricityReadingDetailsViewModel viewModel = _electricityReadingMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создать новую запись показания счетчика электроэнергии.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ElectricityReadingCreatedViewModel>> Create([FromBody] CreateElectricityReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateElectricityReadingDto createDto = _electricityReadingMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ElectricityReadingCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            ElectricityReadingCreatedViewModel resultViewModel = _electricityReadingMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }

        /// <summary>
        /// Отредактировать существующие данные показания счетчика электроэнергии.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ElectricityReadingUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditElectricityReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditElectricityReadingDto editDto = _electricityReadingMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ElectricityReadingCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            ElectricityReadingUpdatedViewModel resultViewModel = _electricityReadingMapper.ToUpdatedViewModel(updatedDto);

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
