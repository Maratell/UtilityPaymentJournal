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
        private readonly ISender _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        /// <summary>
        /// Получить список жилых объектов
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetResidencesListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetResidencesListResponse response = await _mediator.Send(new GetResidencesListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали жилого объекта по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID жилого объекта.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetResidenceByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetResidenceByIdResponse response = await _mediator.Send(new GetResidenceByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись жилого объекта.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateResidenceResponse>> Create(
            [FromBody] CreateResidenceRequest request,
            CancellationToken cancellationToken)
        {
            CreateResidenceResponse response = await _mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные жилого объекта.
        /// </summary>
        /// <param name="id">ID жилого объекта.</param>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditResidenceResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditResidenceRequest request,
            CancellationToken cancellationToken)
        {
            EditResidenceResponse response = await _mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись жилого объекта.
        /// </summary>
        /// <param name="id">ID жилого объекта</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteResidenceCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
