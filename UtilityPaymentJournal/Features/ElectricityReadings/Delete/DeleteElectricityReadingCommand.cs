using MediatR;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Delete
{
    /// <summary>
    /// Команда на удаление показания счетчика электроэнергии.
    /// </summary>
    /// <param name="Id">ID удаляемой записи</param>
    public record DeleteElectricityReadingCommand(long Id) : IRequest;
}
