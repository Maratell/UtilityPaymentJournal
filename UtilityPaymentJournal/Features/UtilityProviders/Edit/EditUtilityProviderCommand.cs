using MediatR;

namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    /// <summary>
    /// Команда на редактирование данных поставщика услуг.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика услуг.</param>
    /// <param name="Name">Новое наименование поставщика услуг.</param>
    public record EditUtilityProviderCommand(long Id, string Name) : IRequest<EditUtilityProviderResponse>;
}
