using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.WaterReadings.Create;
using UtilityPaymentJournal.Features.WaterReadings.Delete;
using UtilityPaymentJournal.Features.WaterReadings.Edit;
using UtilityPaymentJournal.Features.WaterReadings.GetById;
using UtilityPaymentJournal.Features.WaterReadings.GetList;

namespace UtilityPaymentJournal.Features.WaterReadings
{
    /// <summary>
    /// Api-контроллер для управления показаниями счетчиков воды.
    /// </summary>
    [ApiController]
    [Route("api/water-readings")]
    public partial class WaterReadingsApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список показаний счетчиков воды
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию показаний счетчиков воды.</returns>
        [HttpGet]
        public async Task<ActionResult<GetWaterReadingsListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetWaterReadingsListResponse response = await mediator.Send(new GetWaterReadingsListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали показания счетчиков воды по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID показания счетчиков воды.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о показании счетчика воды.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetWaterReadingByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetWaterReadingByIdResponse response = await mediator.Send(new GetWaterReadingByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись показания счетчиков воды.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного показания счетчиков воды. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateWaterReadingResponse>> Create(
            [FromBody] CreateWaterReadingRequest request,
            CancellationToken cancellationToken)
        {
            CreateWaterReadingResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные показания счетчиков воды.
        /// </summary>
        /// <param name="id">Уникальный идентификатор показания счетчиков воды (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленного показания счетчика воды со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditWaterReadingResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditWaterReadingRequest request,
            CancellationToken cancellationToken)
        {
            EditWaterReadingResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись показания счетчиков воды.
        /// </summary>
        /// <param name="id">ID показания счетчиков воды</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteWaterReadingCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
