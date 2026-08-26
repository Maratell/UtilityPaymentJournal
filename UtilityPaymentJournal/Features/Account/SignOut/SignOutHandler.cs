using MediatR;
using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Account.SignOut
{
    /// <summary>
    /// Обработчик команды выхода пользователя из системы (Деаутентификация).
    /// Завершает активную сессию в Identity, очищает файлы куки (cookies) и логирует событие.
    /// </summary>
    public partial class SignOutHandler(
            SignInManager<User> signInManager,
            ILogger<SignOutHandler> logger) : IRequestHandler<SignOutCommand>
    {
        public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            await signInManager.SignOutAsync();
            LogUserSignedOut(logger);
        }
    }
}
