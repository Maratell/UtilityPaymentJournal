using MediatR;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetById
{
    /// <summary>
    /// Запрос на получение развернутых деталей одной записи показания счетчика электроэнергии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор запрашиваемого показания счетчика электроэнергии.</param>
    public record GetElectricityReadingByIdQuery(long Id) : IRequest<GetElectricityReadingByIdResponse>;
}
