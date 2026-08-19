using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Residences.Create;
using UtilityPaymentJournal.Features.Residences.Delete;
using UtilityPaymentJournal.Features.Residences.Edit;
using UtilityPaymentJournal.Features.Residences.GetById;
using UtilityPaymentJournal.Features.Residences.GetList;


namespace UtilityPaymentJournal.Features.Residences
{
    /// <summary>
    /// Api-контроллер для управления списом жилых объектов.
    /// </summary>
    [ApiController]
    [Route("api/residences")]
    public class ResidencesApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список жилых объектов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию жилых объектов.</returns>
        [HttpGet]
        public async Task<ActionResult<GetResidencesListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetResidencesListResponse response = await mediator.Send(new GetResidencesListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали жилого объекта по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID жилого объекта.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о жилом объекте.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetResidenceByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetResidenceByIdResponse response = await mediator.Send(new GetResidenceByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись жилого объекта.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного жилого объекта. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateResidenceResponse>> Create(
            [FromBody] CreateResidenceRequest request,
            CancellationToken cancellationToken)
        {
            CreateResidenceResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), "Residences", new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные жилого объекта.
        /// </summary>
        /// <param name="id">Уникальный идентификатор жилого объекта (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленного жилого объекта со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditResidenceResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditResidenceRequest request,
            CancellationToken cancellationToken)
        {
            EditResidenceResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись жилого объекта.
        /// </summary>
        /// <param name="id">ID жилого объекта</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteResidenceCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
