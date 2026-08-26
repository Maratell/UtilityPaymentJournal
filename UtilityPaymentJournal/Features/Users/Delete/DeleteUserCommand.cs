using MediatR;

namespace UtilityPaymentJournal.Features.Users.Delete
{
    /// <summary>
    /// Команда на удаление пользователя из системы.
    /// </summary>
    /// <param name="Id">строковый идентификатор удаляемой записи.</param>
    public record DeleteUserCommand(string Id) : IRequest;
}
