using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.ElectricityReadings.Create;
using UtilityPaymentJournal.Features.ElectricityReadings.Delete;
using UtilityPaymentJournal.Features.ElectricityReadings.Edit;
using UtilityPaymentJournal.Features.ElectricityReadings.GetById;
using UtilityPaymentJournal.Features.ElectricityReadings.GetList;

namespace UtilityPaymentJournal.Features.ElectricityReadings
{
    /// <summary>
    /// АПИ-контроллер для управления показаниями счетчиков электроэнергии.
    /// </summary>
    [ApiController]
    [Route("api/electricity-readings")]
    public class ElectricityReadingsApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получить список показаний счетчиков электроэнергии
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию показаний счетчиков электроэнергии.</returns>
        [HttpGet]
        public async Task<ActionResult<GetElectricityReadingsListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetElectricityReadingsListResponse response = await mediator.Send(new GetElectricityReadingsListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получить развернутые детали показания счетчиков электроэнергии по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID показания счетчиков электроэнергии.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о показании счетчика электроэнергии.</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetElectricityReadingByIdResponse>> GetById(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            GetElectricityReadingByIdResponse response = await mediator.Send(new GetElectricityReadingByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую запись показания счетчиков электроэнергии.
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного показания счетчиков электроэнергии. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного объекта.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreateElectricityReadingResponse>> Create(
            [FromBody] CreateElectricityReadingRequest request,
            CancellationToken cancellationToken)
        {
            CreateElectricityReadingResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Отредактировать существующие данные показания счетчиков электроэнергии.
        /// </summary>
        /// <param name="id">Уникальный идентификатор показания счетчиков электроэнергии (передается в URL маршрута).</param>
        /// <param name="request">Данные формы обновления. Не содержит ID объекта, так как идентификатор извлекается из маршрута.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Данные обновленного показания счетчика электроэнергии со статусом 200 OK.</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EditElectricityReadingResponse>> Edit(
            [FromRoute] long id,
            [FromBody] EditElectricityReadingRequest request,
            CancellationToken cancellationToken)
        {
            EditElectricityReadingResponse response = await mediator.Send(request.ToCommand(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Удалить запись показания счетчиков электроэнергии.
        /// </summary>
        /// <param name="id">ID показания счетчиков электроэнергии</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteElectricityReadingCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
