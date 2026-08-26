using MediatR;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Delete
{
    /// <summary>
    /// Команда на удаление карточки жалобы.
    /// </summary>
    /// <param name="Id">ID удаляемой записи.</param>
    public record DeleteComplaintCommand(long Id) : IRequest;
}
