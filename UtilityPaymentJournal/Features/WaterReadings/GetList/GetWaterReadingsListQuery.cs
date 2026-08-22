using MediatR;

namespace UtilityPaymentJournal.Features.WaterReadings.GetList
{
    /// <summary>
    /// Запрос на получение списка показаний счетчиков воды.
    /// </summary>
    public record GetWaterReadingsListQuery : IRequest<GetWaterReadingsListResponse>;
}
