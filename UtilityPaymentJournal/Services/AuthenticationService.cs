using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Services
{
    /// <summary>
    /// Реализация сервиса аутентификации.
    /// Инкапсулирует в себе работу с механизмами ASP.NET Core Identity.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Метод для проверки логина/пароля и создания сессии пользователя
        /// </summary>
        /// <param name="signInRequestViewModel"></param>
        /// <returns>
        /// Объект <see cref="AuthenticationResultViewModel"/>, содержащий статус успешности операции sSuccess = true. 
        /// В случае неудачи возвращает IsSuccess = false вместе с локализованным текстом ошибки в свойстве ErrorMessage.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AuthenticationResultViewModel> SignInAsync(SignInRequestViewModel signInRequestViewModel)
        {
            if (signInRequestViewModel == null) 
                throw new ArgumentNullException(nameof(signInRequestViewModel));

            SignInResult result = await _signInManager.PasswordSignInAsync(
                signInRequestViewModel.UserName,
                signInRequestViewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            return result switch
            {
                { Succeeded: true } => new AuthenticationResultViewModel
                {
                    IsSuccess = true,
                    Status = SignInResultStatus.Success
                },

                { IsLockedOut: true } => new AuthenticationResultViewModel
                {
                    IsSuccess = false,
                    Status = SignInResultStatus.LockedOut,
                    ErrorMessage = "Аккаунт временно заблокирован."
                },

                { IsNotAllowed: true } => new AuthenticationResultViewModel
                {
                    IsSuccess = false,
                    Status = SignInResultStatus.NotAllowed,
                    ErrorMessage = "Вход не разрешен. Подтвердите ваш Email."
                },

                _ => new AuthenticationResultViewModel
                {
                    IsSuccess = false,
                    Status = SignInResultStatus.InvalidCredentials,
                    ErrorMessage = "Неверный логин или пароль."
                }
            };
        }

        /// <summary>
        /// Асинхронно завершает текущую сессию пользователя в приложении.
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
