using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Account.GetCurrentUser;
using UtilityPaymentJournal.Features.Account.SignIn;
using UtilityPaymentJournal.Features.Account.SignOut;

namespace UtilityPaymentJournal.Features.Account
{
    [ApiController]
    [Route("api/account")]
    public class AccountApiController(IMediator mediator) : ControllerBase
    {
        [HttpPost("sign-in")]
        [AllowAnonymous] // Разрешает доступ неавторизованным гостям
        // Форсирует проверку антиподделочного токена (CSRF) для точки входа.
        // Защищает от отключения глобального фильтра при будущем рефакторинге (подстраховка)
        // и гарантирует безопасность эндпоинта, открытого для анонимных пользователей [AllowAnonymous].
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SignInResponse>> SignIn(
            [FromBody] SignInRequest request, 
            CancellationToken cancellationToken = default)
        {
            SignInResponse response = await mediator.Send(request.ToCommand(), cancellationToken);

            return response.Status switch
            {
                // Передаем наполненный объект на фронтенд в случае успешной аутентификации (200 OK)
                SignInResultStatus.Success => Ok(response),
                // 401 Unauthorized для неверных учетных данных
                SignInResultStatus.InvalidCredentials => Unauthorized(response),
                // 400 BadRequest для блокировок из-за привышения попыток входа и ограничений доступа
                SignInResultStatus.LockedOut => BadRequest(response),
                // Пользователь заблокирован администратором или доступ ограничен бизнес-логикой
                SignInResultStatus.NotAllowed => BadRequest(response),
                // Защитный дефолтный вариант на случай любых непредвиденных статусов
                _ => BadRequest(response)
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
            await mediator.Send(new SignOutCommand(), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Получить подробную информацию о текущем аутентифицированном пользователе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Модель представления данных профиля текущего пользователя</returns>
        [HttpGet("current")] // Защищен глобальной политикой FallbackPolicy, анонимы сюда не пройдут
        public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        {
            GetCurrentUserResponse response = await mediator.Send(new GetCurrentUserQuery(), cancellationToken);
            return Ok(response);
        }
    }


    //[ApiController]
    //[Route("api/account")]
    //public class AccountApiController : ControllerBase
    //{
    //    private readonly IAuthenticationCommandService _authenticationCommandService;
    //    private readonly IAuthenticationQueryService _authenticationQueryService;
    //    private readonly IAccountMapper _accountMapper;

    //    public AccountApiController(
    //        IAuthenticationCommandService authenticationCommandService,
    //        IAuthenticationQueryService authenticationQueryService,
    //        IAccountMapper accountMapper)
    //    {
    //        _authenticationCommandService = authenticationCommandService ?? throw new ArgumentNullException(nameof(authenticationCommandService));
    //        _authenticationQueryService = authenticationQueryService ?? throw new ArgumentNullException(nameof(authenticationQueryService));
    //        _accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
    //    }

    //    [HttpPost("sign-in")]
    //    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    //    // Форсирует проверку антиподделочного токена (CSRF) для точки входа.
    //    // Защищает от отключения глобального фильтра при будущем рефакторинге (подстраховка)
    //    // и гарантирует безопасность эндпоинта, открытого для анонимных пользователей [AllowAnonymous].
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> SignIn([FromBody] SignInRequestViewModel signInRequestViewModel, CancellationToken cancellationToken = default)
    //    {
    //        SignInDto signInDto = _accountMapper.ToSignInDto(signInRequestViewModel);
    //        AuthenticationCommandResultDto authenticationResultDto = await _authenticationCommandService.SignInAsync(signInDto, cancellationToken);
    //        UserSignedInViewModel authenticationResultViewModel = _accountMapper.ToSignedInViewModel(authenticationResultDto);

    //        // Если вход успешен, генерируем путь перенаправления
    //        if (authenticationResultViewModel.Status == SignInResultStatus.Success)
    //        {
    //            authenticationResultViewModel.RedirectUrl = Url.Action("Index", "Home", null, Request.Scheme);
    //        }
    //        return authenticationResultViewModel.Status switch
    //        {
    //            // Передаем наполненный объект с RedirectUrl на фронтенд (200 OK)
    //            SignInResultStatus.Success => Ok(authenticationResultViewModel),

    //            // 401 Unauthorized для неверных учетных данных
    //            SignInResultStatus.InvalidCredentials => Unauthorized(authenticationResultViewModel),

    //            // 400 BadRequest для блокировок из-за привышения попыток входа и ограничений доступа
    //            SignInResultStatus.LockedOut => BadRequest(authenticationResultViewModel),

    //            // Пользователь заблокирован администратором или доступ ограничен бизнес-логикой
    //            SignInResultStatus.NotAllowed => BadRequest(authenticationResultViewModel),

    //            // Защитный дефолтный вариант на случай любых непредвиденных статусов
    //            _ => BadRequest(authenticationResultViewModel)
    //        };
    //    }

    //    /// <summary>
    //    /// Выход пользователя из системы (завершение текущей сессии).
    //    /// </summary>
    //    /// <param name="cancellationToken">Токен отмены операции.</param>
    //    [HttpPost("sign-out")]
    //    // Форсирует проверку антиподделочного токена (CSRF) для выхода.
    //    // Защищает от отключения глобального фильтра при будущем рефакторинге (подстраховка).
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
    //    {
    //        // 1. Завершаем сессию 
    //        await _authenticationCommandService.SignOutAsync(cancellationToken);

    //        // 2. Формируем ViewModel ответа и указываем страницу, куда нужно перенаправить пользователя после выхода
    //        UserSignedOutViewModel signedOutViewModel = new UserSignedOutViewModel
    //        {
    //            IsSuccess = true,
    //            RedirectUrl = Url.Action("Index", "Account", null, Request.Scheme)
    //        };

    //        return Ok(signedOutViewModel);
    //    }

    //    /// <summary>
    //    /// Получить подробную информацию о текущем аутентифицированном пользователе.
    //    /// </summary>
    //    /// <param name="cancellationToken">Токен отмены операции</param>
    //    /// <returns>Модель представления данных профиля текущего пользователя</returns>
    //    [HttpGet("current")] // Защищен глобальной политикой FallbackPolicy, анонимы сюда не пройдут
    //    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    //    {
    //        CurrentUserQueryResultDto queryResultDto = await _authenticationQueryService.GetCurrentUserDetailsAsync(cancellationToken);
    //        CurrentUserDetailsViewModel detailsViewModel = _accountMapper.ToDetailsViewModel(queryResultDto);

    //        return Ok(detailsViewModel);
    //    }
    //}
}
