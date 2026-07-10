using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.DTOs.Account;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    /// <summary>
    /// Реализация сервиса аутентификации.
    /// Инкапсулирует в себе работу с механизмами ASP.NET Core Identity.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountMapper _accountMapper;

        public AuthenticationService(
            SignInManager<User> signInManager,
            IAccountMapper accountMapper)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        /// <summary>
        /// Метод для проверки логина/пароля и создания сессии пользователя (без использования Email).
        /// </summary>
        /// <param name="signInDto">Входное Dto с учетными данными пользователя.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Объект <see cref="AuthenticationResultDTO"/>, содержащий статус успешности операции и данные для маппинга.
        /// </returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект DTO равен null.</exception>
        public async Task<AuthenticationResultDTO> SignInAsync(SignInDto signInDto, CancellationToken cancellationToken = default)
        {
            if (signInDto == null)
                throw new ArgumentNullException(nameof(signInDto));

            cancellationToken.ThrowIfCancellationRequested();

            // Проверяем подлинность учетных данных по UserName и Password.
            SignInResult result = await _signInManager.PasswordSignInAsync(
                signInDto.UserName,
                signInDto.Password,
                isPersistent: signInDto.IsPersistent, // Флаг запоминания сессии при выходе из браузера
                lockoutOnFailure: false // Отключаем блокировку при многократных ошибках ввода
            );

            cancellationToken.ThrowIfCancellationRequested();

            return _accountMapper.ToDto(result);
        }

        /// <summary>
        /// Асинхронно завершает текущую сессию пользователя в приложении.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        public async Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Удаляем куки аутентификации пользователя
            await _signInManager.SignOutAsync();
        }
    }
}
