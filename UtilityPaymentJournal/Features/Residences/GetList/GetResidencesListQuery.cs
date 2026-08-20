using MediatR;

namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Запрос на получение списка объектов недвижимости.
    /// </summary>
    public record GetResidencesListQuery : IRequest<GetResidencesListResponse>;
}
