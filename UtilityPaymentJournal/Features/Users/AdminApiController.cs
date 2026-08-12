using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Users.Commands;
using UtilityPaymentJournal.Features.Users.Models;
using UtilityPaymentJournal.Features.Users.Queries;

namespace UtilityPaymentJournal.Features.Users
{
    /// <summary>
    /// API-контроллер для административного управления пользователями и ролями
    /// </summary>
    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    [ApiController]
    [Route("api/admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly IUserQueryService _queryService;
        private readonly IUserCommandService _commandService;
        private readonly IUserMapper _userMapper;

        public AdminApiController(
            IUserQueryService queryService,
            IUserCommandService commandService,
            IUserMapper userMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        }

        /// <summary>
        /// Получение списка всех пользователей системы
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyCollection<UserDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<UserQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);

            UserDetailsViewModel[] viewModels = dtos
                .Select(u => _userMapper.ToViewModel(u))
                .ToArray();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получение данных конкретного пользователя по его ID
        /// </summary>
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDetailsViewModel>> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в KeyNotFoundExceptionHandler)
            UserQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            UserDetailsViewModel viewModel = _userMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создание нового пользователя и автоматическое назначение ему выбранной роли
        /// </summary>
        [HttpPost("users")]
        public async Task<ActionResult<UserCreatedViewModel>> CreateUserWithRole(
            [FromBody] CreateUserViewModel createUserVM,
            CancellationToken cancellationToken)
        {
            CreateUserDto createDto = _userMapper.ToDto(createUserVM);
            // При отсутствии объекта сервис выбросит IdentityValidationException (обработается в IdentityValidationExceptionHandler)
            UserCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            UserCreatedViewModel resultViewModel = _userMapper.ToCreatedViewModel(createdDto);

            return Ok(resultViewModel);
        }

        /// <summary>
        /// Удаление пользователя из системы
        /// </summary>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            // Если пользователя нет — выбросится исключение, которое  внешний ExceptionHandler обработает .
            await _commandService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
