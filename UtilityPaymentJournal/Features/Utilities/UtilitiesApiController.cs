using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Utilities.Create;
using UtilityPaymentJournal.Features.Utilities.Delete;
using UtilityPaymentJournal.Features.Utilities.Edit;
using UtilityPaymentJournal.Features.Utilities.GetById;
using UtilityPaymentJournal.Features.Utilities.GetList;

namespace UtilityPaymentJournal.Features.Utilities
{
    /// <summary>
    /// Api-контроллер для управления коммунальными услугами.
    /// </summary>
    [ApiController]
    [Route("api/utilities")]
    public class UtilitiesApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список услуг
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию услуг.</returns>
        [HttpGet]
        public async Task<ActionResult<GetUtilitiesListResponse>> GetAll(
            [FromQuery] GetUtilitiesListQuery query, 
            CancellationToken cancellationToken)
        {
            GetUtilitiesListResponse response = await mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали услуги по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID услуги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией об услуги.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetUtilityByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetUtilityByIdResponse response = await mediator.Send(new GetUtilityByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись услуги.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданноой услуге. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateUtilityResponse>> Create(
            [FromBody] CreateUtilityRequest request,
            CancellationToken cancellationToken)
        {
            CreateUtilityResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные услуги.
        /// </summary>
        /// <param name="id">Уникальный идентификатор услуги (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленной услуги со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditUtilityResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditUtilityRequest request,
            CancellationToken cancellationToken)
        {
            EditUtilityResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись услуги.
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteUtilityCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
