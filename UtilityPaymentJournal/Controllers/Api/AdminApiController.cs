using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Admin;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Controllers.Api
{
    /// <summary>
    /// API-контроллер для административного управления пользователями и ролями
    /// </summary>
    [ApiController]
    [Route("api/admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;

        public AdminApiController(IUserService userService, IUserMapper userMapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        }

        /// <summary>
        /// Получение списка всех пользователей системы
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyCollection<UserViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<UserDTO> dtos = await _userService.GetAllAsync(cancellationToken);

            List<UserViewModel> viewModels = dtos
                .Select(dto => _userMapper.ToViewModel(dto))
                .ToList();

            return Ok(viewModels);
        }

        /// <summary>
        /// Получение данных конкретного пользователя по его ID
        /// </summary>
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserViewModel>> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            UserDTO? dto = await _userService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            UserViewModel viewModel = _userMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        /// <summary>
        /// Создание нового пользователя и автоматическое назначение ему выбранной роли
        /// </summary>
        [HttpPost("users")]
        public async Task<ActionResult<UserViewModel>> CreateUserWithRole(
            [FromBody] CreateUserViewModel createUserVM,
            CancellationToken cancellationToken)
        {
            CreateUserDTO createDto = _userMapper.ToDto(createUserVM);

            UserDTO createdDto = await _userService.CreateAsync(createDto, cancellationToken);

            UserViewModel createdViewModel = _userMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        /// <summary>
        /// Удаление пользователя из системы
        /// </summary>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _userService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Пользователь с ID {id} не найден.");
            }

            return NoContent(); 
        }
    }
}
