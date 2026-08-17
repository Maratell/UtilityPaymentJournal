using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Residences.Delete
{
    /// <summary>
    /// Команда на удаление объекта недвижимости.
    /// </summary>
    /// <param name="Id">ID удаляемой записи</param>
    public record DeleteResidenceCommand(long Id) : IRequest;
}
