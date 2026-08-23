using MediatR;

namespace UtilityPaymentJournal.Features.Utilities.Delete
{
    /// <summary>
    /// Команда на удаление услуги.
    /// </summary>
    /// <param name="Id">ID удаляемой записи.</param>
    public record DeleteUtilityCommand(long Id) : IRequest;
}
