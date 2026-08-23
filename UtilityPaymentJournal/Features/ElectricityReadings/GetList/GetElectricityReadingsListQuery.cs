using MediatR;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetList
{
    /// <summary>
    /// Запрос на получение списка показаний счетчиков электроэнергии.
    /// </summary>
    public record GetElectricityReadingsListQuery : IRequest<GetElectricityReadingsListResponse>;
}
