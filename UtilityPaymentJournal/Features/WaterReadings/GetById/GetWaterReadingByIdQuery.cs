using MediatR;

namespace UtilityPaymentJournal.Features.WaterReadings.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной записи показания счетчика воды.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемого показания счетчика воды.</param>
    public record GetWaterReadingByIdQuery(long Id) : IRequest<GetWaterReadingByIdResponse>;
}
