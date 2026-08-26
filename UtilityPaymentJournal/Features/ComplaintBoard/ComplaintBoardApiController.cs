using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.ComplaintBoard.Create;
using UtilityPaymentJournal.Features.ComplaintBoard.Delete;
using UtilityPaymentJournal.Features.ComplaintBoard.Edit;
using UtilityPaymentJournal.Features.ComplaintBoard.GetById;
using UtilityPaymentJournal.Features.ComplaintBoard.GetList;
using UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus;

namespace UtilityPaymentJournal.Features.ComplaintBoard
{
    /// <summary>
    /// Api-контроллер для управления жалобами.
    /// </summary>
    [ApiController]
    [Route("api/complaint-board")]
    public class ComplaintBoardApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список карточек жалоб
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию карточек жалоб.</returns>
        [HttpGet]
        public async Task<ActionResult<GetComplaintsListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetComplaintsListResponse response = await mediator.Send(new GetComplaintsListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали карточки с жалобой по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID карточки с жалобой.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о карточке с жалобой.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetComplaintByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetComplaintByIdResponse response = await mediator.Send(new GetComplaintByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись карточки с жалобой.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного карточки с жалобой. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateComplaintResponse>> Create(
            [FromBody] CreateComplaintRequest request,
            CancellationToken cancellationToken)
        {
            CreateComplaintResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные карточки с жалобой.
        /// </summary>
        /// <param name="id">Уникальный идентификатор карточки с жалобой (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленного карточки с жалобой со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditComplaintResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditComplaintRequest request,
            CancellationToken cancellationToken)
        {
            EditComplaintResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Изменить статус существующей карточки с жалобой.
        /// </summary>
        [HttpPatch("{id:long}/change-status")]
        public async Task<ActionResult<ChangeComplaintStatusResponse>> ChangeStatus(
            [FromRoute] long id,
            [FromBody] ChangeComplaintStatusRequest request, 
            CancellationToken cancellationToken)
        {
            ChangeComplaintStatusResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись карточки с жалобой.
        /// </summary>
        /// <param name="id">ID карточки с жалобой</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteComplaintCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
