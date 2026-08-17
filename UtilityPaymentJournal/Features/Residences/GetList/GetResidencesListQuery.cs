using MediatR;

namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Запрос на получение списка всех объектов недвижимости.
    /// </summary>
    public record GetResidencesListQuery : IRequest<GetResidencesListResponse>;
}
