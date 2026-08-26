using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus
{
    /// <summary>
    /// Запрос на создание карточки жалобы
    /// </summary>
    /// <param name="NewStatus">Новый статус рассмотрения жалобы</param>
    public record ChangeComplaintStatusRequest(
        ComplaintStatus NewStatus
    )
    {
        public ChangeComplaintStatusCommand ToCommand(long id) =>
            new ChangeComplaintStatusCommand(id, NewStatus);
    }
}
