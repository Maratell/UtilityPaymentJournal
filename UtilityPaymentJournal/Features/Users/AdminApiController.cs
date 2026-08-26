using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Users.Create;
using UtilityPaymentJournal.Features.Users.Delete;
using UtilityPaymentJournal.Features.Users.GetById;
using UtilityPaymentJournal.Features.Users.GetList;

namespace UtilityPaymentJournal.Features.Users
{
    /// <summary>
    /// Api-контроллер для административного управления пользователями и ролями
    /// </summary>
    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    [ApiController]
    [Route("api/admin")]
    public class AdminApiController(ISender mediator) : ControllerBase
    {
        /// <summary>
        /// Получение списка всех пользователей системы
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект-обёртку, содержащий коллекцию пользователей.</returns>
        [HttpGet("users")]
        public async Task<ActionResult<GetUsersListResponse>> GetAll(CancellationToken cancellationToken)
        {
            GetUsersListResponse response = await mediator.Send(new GetUsersListQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных конкретного пользователя по его ID
        /// </summary>
        /// <param name="id">Строковый идентификатор пользователя в системе.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статус 200 OK и объект с подробной информацией о пользователе.</returns>
        [HttpGet("users/{id}")]
        public async Task<ActionResult<GetUserByIdResponse>> GetById(
            [FromRoute] string id, 
            CancellationToken cancellationToken)
        {
            GetUserByIdResponse response = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Создание нового пользователя и автоматическое назначение ему выбранной роли
        /// </summary>
        /// <param name="request">Данные формы с фронтенда.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Статус 201 Created и данные созданного в системе пользователя. 
        /// В заголовке Location ответа возвращается URL для получения деталей созданного пользователя.
        /// </returns>
        [HttpPost("users")]
        public async Task<ActionResult<CreateUserResponse>> CreateUserWithRole(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            CreateUserResponse response = await mediator.Send(request.ToCommand(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Удаление пользователя из системы
        /// </summary>
        /// <param name="id">Строковый идентификатор пользователя в системе.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Статус 204 No Content в случае успешного удаления (тело ответа отсутствует).</returns>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] string id, 
            CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteUserCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
