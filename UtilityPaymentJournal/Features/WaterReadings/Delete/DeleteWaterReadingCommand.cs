using MediatR;

namespace UtilityPaymentJournal.Features.WaterReadings.Delete
{
    /// <summary>
    /// Команда на удаление показания счетчика воды.
    /// </summary>
    /// <param name="Id">ID удаляемой записи</param>
    public record DeleteWaterReadingCommand(long Id) : IRequest;
}
