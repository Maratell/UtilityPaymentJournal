using MediatR;

namespace UtilityPaymentJournal.Features.Account.SignOut
{
    /// <summary>
    /// Команда на завершение текущей активнуой сессии пользователя с удалением кук (Деаутентификация).
    /// </summary>
    public record SignOutCommand : IRequest;
}
