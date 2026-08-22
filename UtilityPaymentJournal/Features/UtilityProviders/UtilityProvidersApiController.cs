using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.UtilityProviders.Create;
using UtilityPaymentJournal.Features.UtilityProviders.Delete;
using UtilityPaymentJournal.Features.UtilityProviders.Edit;
using UtilityPaymentJournal.Features.UtilityProviders.GetById;
using UtilityPaymentJournal.Features.UtilityProviders.GetList;


namespace UtilityPaymentJournal.Features.UtilityProviders
{
    /// <summary>
    /// Api-контроллер для управления поставщиками услуг.
    /// </summary>
    [ApiController]
    [Route("api/utility-providers")]
    public class UtilityProvidersApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список поставщиков услуг
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию поставщиков услуг.</returns>
        [HttpGet]
        public async Task<ActionResult<GetUtilityProvidersListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetUtilityProvidersListResponse response = await mediator.Send(new GetUtilityProvidersListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали поставщика услуг по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID поставщика услуг.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о поставщике услуг.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetUtilityProviderByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetUtilityProviderByIdResponse response = await mediator.Send(new GetUtilityProviderByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись поставщика услуг.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного поставщика услуг. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateUtilityProviderResponse>> Create(
            [FromBody] CreateUtilityProviderRequest request,
            CancellationToken cancellationToken)
        {
            CreateUtilityProviderResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные поставщика услуг.
        /// </summary>
        /// <param name="id">Уникальный идентификатор поставщика услуг (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленного поставщика услуг со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditUtilityProviderResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditUtilityProviderRequest request,
            CancellationToken cancellationToken)
        {
            EditUtilityProviderResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись поставщика услуг.
        /// </summary>
        /// <param name="id">ID поставщика услуг</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteUtilityProviderCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
