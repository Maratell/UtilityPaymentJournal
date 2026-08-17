using MediatR;

namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// Команда на создание нового объекта недвижимости.
    /// </summary>
    /// <param name="Address">адрес недвижимости.</param>
    public record CreateResidenceCommand(string Address) : IRequest<CreateResidenceResponse>;
}
