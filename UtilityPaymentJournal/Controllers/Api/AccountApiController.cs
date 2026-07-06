using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountApiController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService 
                ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestViewModel signInRequestViewModel)
        {
            AuthenticationResultViewModel authenticationResultViewModel = await _authenticationService.SignInAsync(signInRequestViewModel);

            // Если вход успешен, генерируем путь перенаправления
            if (authenticationResultViewModel.Status == SignInResultStatus.Success)
            {
                authenticationResultViewModel.RedirectUrl = Url.Action("Index", "Home", null, Request.Scheme);
            }
            return authenticationResultViewModel.Status switch
            {
                // Передаем наполненный объект с RedirectUrl на фронтенд
                SignInResultStatus.Success => Ok(authenticationResultViewModel),

                // Для неверного пароля/логина возвращаем 401 (RedirectUrl внутри равен null)
                SignInResultStatus.InvalidCredentials => Unauthorized(authenticationResultViewModel),

                // Для блокировок и системных ограничений возвращаем 400 (RedirectUrl внутри равен null)
                SignInResultStatus.LockedOut or SignInResultStatus.NotAllowed => BadRequest(authenticationResultViewModel),

                // Защитный дефолтный вариант на случай непредвиденных статусов
                _ => BadRequest(new AuthenticationResultViewModel
                {
                    IsSuccess = false,
                    ErrorMessage = "Произошла непредвиденная ошибка при обработке запроса."
                })
            };
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutAsync();

            var result = new AuthenticationResultViewModel
            {
                IsSuccess = true,
                Status = SignInResultStatus.Success,
                RedirectUrl = Url.Action("Index", "Account", null, Request.Scheme)
            };

            return Ok(result);
        }
    }
}
