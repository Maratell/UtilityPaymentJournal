using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Account.Commands
{
    /// <summary>
    /// Сервис команд (записи) для управления сессиями аутентификации.
    /// Инкапсулирует в себе работу с механизмами записи и изменения состояний сессий ASP.NET Core Identity.
    /// </summary>
    public partial class AuthenticationCommandService(
            SignInManager<User> signInManager,
            ILogger<AuthenticationCommandService> logger) : IAuthenticationCommandService
    {
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly ILogger<AuthenticationCommandService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Проверить подлинность учетных данных пользователя и создать активную сессию в системе без использования Email.
        /// </summary>
        /// <param name="signInDto">ДТО с входными учетными данными пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО результата выполнения команды со статусом операции и сообщением об ошибке</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект DTO равен null</exception>
        public async Task<AuthenticationCommandResultDto> SignInAsync(SignInDto signInDto, CancellationToken cancellationToken = default)
        {
            // Выполняем проверку подлинности по UserName и Password
            SignInResult result = await _signInManager.PasswordSignInAsync(
                signInDto.UserName,
                signInDto.Password,
                isPersistent: signInDto.IsPersistent,
                lockoutOnFailure: true
            );

            // Успешная аутентификация
            if (result.Succeeded)
            {
                LogUserSignedIn(_logger, signInDto.UserName);
                return new AuthenticationCommandResultDto(IsSuccess: true, Status: SignInResultStatus.Success);
            }

            // Аккаунт заблокирован из-за множества ошибок ввода
            if (result.IsLockedOut)
            {
                LogUserLockedOut(_logger, signInDto.UserName);
                return new AuthenticationCommandResultDto(
                    IsSuccess: false,
                    Status: SignInResultStatus.LockedOut,
                    ErrorMessage: "Аккаунт временно заблокирован из-за множества неудачных попыток входа."
                );
            }

            // Доступ запрещен администратором или правилами бизнес-логики
            if (result.IsNotAllowed)
            {
                LogUserLoginNotAllowed(_logger, signInDto.UserName);
                return new AuthenticationCommandResultDto(
                    IsSuccess: false,
                    Status: SignInResultStatus.NotAllowed,
                    ErrorMessage: "Доступ к системе ограничен. Обратитесь к администратору."
                );
            }

            // Общая ошибка (неверный логин или пароль)
            LogUserSignInFailed(_logger, signInDto.UserName);
            return new AuthenticationCommandResultDto(
                IsSuccess: false,
                Status: SignInResultStatus.InvalidCredentials,
                ErrorMessage: "Неверный логин или пароль."
            );
        }

        /// <summary>
        /// Асинхронно завершить текущую активную сессию пользователя в приложении с удалением кук.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию выхода</returns>
        public async Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();
            LogUserSignedOut(_logger);
        }
    }
}
