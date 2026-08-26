using MediatR;
using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Exceptions;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.Delete
{
    /// <summary>
    /// Обработчик команды удаления пользователя из системы.
    /// </summary>
    public partial class DeleteUserHandler(
            UserManager<User> userManager,
            ILogger<DeleteUserHandler> logger) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            LogUserDeletionRequested(logger, command.Id);

            User? user = await userManager.FindByIdAsync(command.Id);
            if (user == null)
            {
                LogUserToDeleteNotFound(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось найти пользователя с ID {command.Id}.");
            }

            IdentityResult result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                LogUserDeletionFailed(logger, command.Id);
                throw new IdentityValidationException(result.Errors.Select(e => e.Description));
            }

            LogUserSuccessfullyDeleted(logger, command.Id, user.UserName ?? string.Empty);
        }
    }
}
