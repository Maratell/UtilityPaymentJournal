using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountMapper _accountMapper;

        public AccountApiController(
            IAuthenticationService authenticationService,
            IAccountMapper accountMapper)
        {
            _authenticationService = authenticationService 
                ?? throw new ArgumentNullException(nameof(authenticationService));

            _accountMapper = accountMapper
                ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        [HttpPost("sign-in")]
        // Форсирует проверку антиподделочного токена (CSRF) для точки входа.
        // Защищает от отключения глобального фильтра при будущем рефакторинге (подстраховка)
        // и гарантирует безопасность эндпоинта, открытого для анонимных пользователей [AllowAnonymous].
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestViewModel signInRequestViewModel, CancellationToken cancellationToken = default)
        {
            SignInDto signInDto = _accountMapper.ToSignInDto(signInRequestViewModel);
            AuthenticationResultDto authenticationResultDto = await _authenticationService.SignInAsync(signInDto, cancellationToken);
            AuthenticationResultViewModel authenticationResultViewModel = _accountMapper.ToViewModel(authenticationResultDto);

            // Если вход успешен, генерируем путь перенаправления
            if (authenticationResultViewModel.Status == SignInResultStatus.Success)
            {
                authenticationResultViewModel.RedirectUrl = Url.Action("Index", "Home", null, Request.Scheme);
            }
            return authenticationResultViewModel.Status switch
            {
                // Передаем наполненный объект с RedirectUrl на фронтенд (200 OK)
                SignInResultStatus.Success => Ok(authenticationResultViewModel),

                // 401 Unauthorized для неверных учетных данных
                SignInResultStatus.InvalidCredentials => Unauthorized(authenticationResultViewModel),

                // 400 BadRequest для блокировок из-за привышения попыток входа и ограничений доступа
                SignInResultStatus.LockedOut => BadRequest(authenticationResultViewModel),

                // Пользователь заблокирован администратором или доступ ограничен бизнес-логикой
                SignInResultStatus.NotAllowed => BadRequest(authenticationResultViewModel),

                // Защитный дефолтный вариант на случай любых непредвиденных статусов
                _ => BadRequest(authenticationResultViewModel)
            };
        }

        /// <summary>
        /// Выход пользователя из системы (завершение текущей сессии).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        [HttpPost("sign-out")]
        // Форсирует проверку антиподделочного токена (CSRF) для выхода.
        // Защищает от отключения глобального фильтра при будущем рефакторинге (подстраховка).
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
        {
            // Завершаем сессию 
            await _authenticationService.SignOutAsync(cancellationToken);

            // Формируем ViewModel ответа и указываем страницу, куда нужно перенаправить пользователя после выхода
            AuthenticationResultViewModel authenticationResultViewModel = new AuthenticationResultViewModel
            {
                IsSuccess = true,
                Status = SignInResultStatus.Success,
                RedirectUrl = Url.Action("Index", "Account", null, Request.Scheme)
            };

            return Ok(authenticationResultViewModel);
        }
    }
}
