using MediatR;

namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    /// <summary>
    /// Команда на создание нового поставщика услуг
    /// </summary>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record CreateUtilityProviderCommand(string Name) : IRequest<CreateUtilityProviderResponse>;
}
