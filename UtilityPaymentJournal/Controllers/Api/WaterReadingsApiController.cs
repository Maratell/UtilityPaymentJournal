using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.WaterReadings.Commands;
using UtilityPaymentJournal.Features.WaterReadings.Models;
using UtilityPaymentJournal.Features.WaterReadings.Queries;
using UtilityPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers.Api
{
    /// <summary>
    /// АПИ-контроллер для управления показаниями счетчиков воды.
    /// </summary>
    [ApiController]
    [Route("api/water-readings")]
    public partial class WaterReadingsApiController : ControllerBase
    {
        private readonly IWaterReadingQueryService _queryService;
        private readonly IWaterReadingCommandService _commandService;
        private readonly IWaterReadingMapper _waterReadingMapper;

        public WaterReadingsApiController(
            IWaterReadingQueryService queryService,
            IWaterReadingCommandService commandService,
            IWaterReadingMapper waterReadingMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _waterReadingMapper = waterReadingMapper ?? throw new ArgumentNullException(nameof(waterReadingMapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<WaterReadingDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            // Используем сервис запросов (Queries) для получения полного списка со всеми Include
            IReadOnlyCollection<WaterReadingQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);
            WaterReadingDetailsViewModel[] viewModels = dtos
                .Select(r => _waterReadingMapper.ToViewModel(r))
                .ToArray();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получить развернутые детали показания счетчика воды по его уникальному идентификатору.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<WaterReadingDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            WaterReadingQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            WaterReadingDetailsViewModel viewModel = _waterReadingMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создать новую запись показания счетчика воды.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<WaterReadingCreatedViewModel>> Create([FromBody] CreateWaterReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateWaterReadingDto createDto = _waterReadingMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            WaterReadingCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            WaterReadingCreatedViewModel resultViewModel = _waterReadingMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }

        /// <summary>
        /// Отредактировать существующие данные показания счетчика воды.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<WaterReadingUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditWaterReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditWaterReadingDto editDto = _waterReadingMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            WaterReadingCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            WaterReadingUpdatedViewModel resultViewModel = _waterReadingMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }

        /// <summary>
        /// Удалить запись показания счетчика воды из системы.
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
