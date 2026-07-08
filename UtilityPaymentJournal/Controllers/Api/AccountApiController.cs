using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTO.Account;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Controllers.Api
{
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
        public async Task<IActionResult> SignIn([FromBody] SignInRequestViewModel signInRequestViewModel, CancellationToken cancellationToken = default)
        {
            SignInDto signInDto = _accountMapper.ToDto(signInRequestViewModel);

            AuthenticationResultDTO authenticationResultDto = await _authenticationService.SignInAsync(signInDto, cancellationToken);

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

                // Для неверного пароля/логина возвращаем 401 Unauthorized
                SignInResultStatus.InvalidCredentials => Unauthorized(authenticationResultViewModel),

                // Для блокировок из-за превышения попыток ввода возвращаем 400 BadRequest (убрали NotAllowed)
                SignInResultStatus.LockedOut => BadRequest(authenticationResultViewModel),

                // Защитный дефолтный вариант на случай любых непредвиденных статусов
                _ => BadRequest(new AuthenticationResultViewModel
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось выполнить вход. Пожалуйста, обратитесь к администратору."
                })
            };
        }

        /// <summary>
        /// Выход пользователя из системы (завершение текущей сессии).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        [HttpPost("sign-out")]
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
