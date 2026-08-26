using MediatR;
using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Account.SignIn
{
    /// <summary>
    /// Обработчик команды входа пользователя в систему (Аутентификация).
    /// Выполняет проверку учетных данных через Identity и возвращает структурированный результат со статусом операции.
    /// </summary>
    public partial class SignInHandler(
        SignInManager<User> signInManager,
        ILogger<SignInHandler> logger) : IRequestHandler<SignInCommand, SignInResponse>
    {
        public async Task<SignInResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            // Выполняем проверку подлинности по UserName и Password
            SignInResult result = await signInManager.PasswordSignInAsync(
                userName: command.UserName,
                password: command.Password,
                isPersistent: command.IsPersistent,
                lockoutOnFailure: true
            );
            
            if (result.Succeeded) // Успешная аутентификация
            {
                LogUserSignedIn(logger, command.UserName);
                return SignInResponse.Success();
            }

            if (result.IsLockedOut) // Аккаунт заблокирован из-за множества ошибок ввода
            {
                LogUserLockedOut(logger, command.UserName);
                return SignInResponse.LockedOut();
            }

            
            if (result.IsNotAllowed) // Доступ запрещен администратором или правилами бизнес-логики
            {
                LogUserLoginNotAllowed(logger, command.UserName);
                return SignInResponse.NotAllowed();
            }

            // Общая ошибка (неверный логин или пароль)
            LogUserSignInFailed(logger, command.UserName);
            return SignInResponse.InvalidCredentials();
        }
    }
}
