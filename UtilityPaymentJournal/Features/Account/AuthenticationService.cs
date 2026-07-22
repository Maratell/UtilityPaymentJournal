using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    /// <summary>
    /// Реализация сервиса аутентификации.
    /// Инкапсулирует в себе работу с механизмами ASP.NET Core Identity.
    /// </summary>
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            SignInManager<User> signInManager,
            ILogger<AuthenticationService> logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Метод для проверки логина/пароля и создания сессии пользователя (без использования Email).
        /// </summary>
        /// <param name="signInDto">Входное Dto с учетными данными пользователя.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>
        /// Объект <see cref="AuthenticationResultDto"/>, содержащий статус успешности операции и данные для маппинга.
        /// </returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект DTO равен null.</exception>
        public async Task<AuthenticationResultDto> SignInAsync(SignInDto signInDto, CancellationToken cancellationToken = default)
        {
            if (signInDto == null)
                throw new ArgumentNullException(nameof(signInDto));

            // Проверяем подлинность учетных данных по UserName и Password.
            SignInResult result = await _signInManager.PasswordSignInAsync(
                signInDto.UserName,
                signInDto.Password,
                isPersistent: signInDto.IsPersistent, // Флаг запоминания сессии при выходе из браузера
                lockoutOnFailure: true // Включаем блокировку при многократных ошибках ввода
            );

            // 1. Успешная аутентификация
            if (result.Succeeded)
            {
                LogUserSignedIn(_logger, signInDto.UserName);
                return new AuthenticationResultDto(IsSuccess: true, Status: SignInResultStatus.Success);
            }

            // 2. Аккаунт заблокирован из-за перебора паролей
            if (result.IsLockedOut)
            {
                LogUserLockedOut(_logger, signInDto.UserName);
                return new AuthenticationResultDto(
                    IsSuccess: false,
                    Status: SignInResultStatus.LockedOut,
                    ErrorMessage: "Аккаунт временно заблокирован из-за множества неудачных попыток входа."
                );
            }

            // 3. Пользователю запрещен вход администратором / бизнес-логикой
            if (result.IsNotAllowed)
            {
                LogUserLoginNotAllowed(_logger, signInDto.UserName);
                return new AuthenticationResultDto(
                    IsSuccess: false,
                    Status: SignInResultStatus.NotAllowed,
                    ErrorMessage: "Доступ к системе ограничен. Обратитесь к администратору."
                );
            }

            // 4. Общая ошибка (неверный логин или пароль)
            LogUserSignInFailed(_logger, signInDto.UserName);
            return new AuthenticationResultDto(
                IsSuccess: false,
                Status: SignInResultStatus.InvalidCredentials,
                ErrorMessage: "Неверный логин или пароль."
            );
        }

        /// <summary>
        /// Асинхронно завершает текущую сессию пользователя в приложении.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        public async Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            // Удаляем куки аутентификации пользователя
            await _signInManager.SignOutAsync();
        }
    }
}
