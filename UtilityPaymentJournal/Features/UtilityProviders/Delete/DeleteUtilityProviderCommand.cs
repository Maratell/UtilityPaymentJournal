using MediatR;

namespace UtilityPaymentJournal.Features.UtilityProviders.Delete
{
    /// <summary>
    /// Команда на удаление поставщика услуг.
    /// </summary>
    /// <param name="Id">ID удаляемой записи.</param>
    public record DeleteUtilityProviderCommand(long Id) : IRequest;
}
