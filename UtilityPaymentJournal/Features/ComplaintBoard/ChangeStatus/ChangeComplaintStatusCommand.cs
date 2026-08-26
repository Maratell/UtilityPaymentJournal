using MediatR;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus
{
    /// <summary>
    /// Команда на изиенение статуса рассмотрения карточки жалобы
    /// </summary>
    /// <param name="Id">Идентификатор изменяемой карточки жалобы в БД</param>
    /// <param name="NewStatus">Новый статус рассмотрения жалобы</param>
    public record ChangeComplaintStatusCommand(
        long Id,
        ComplaintStatus NewStatus
    ) : IRequest<ChangeComplaintStatusResponse>;
}
