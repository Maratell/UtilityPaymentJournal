using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Команда на редактирование данных объекта недвижимости.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости.</param>
    /// <param name="Address">Новый адрес объекта недвижимости.</param>
    public record EditResidenceCommand(long Id, string Address) : IRequest<EditResidenceResponse>;
}
