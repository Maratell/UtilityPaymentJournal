using MediatR;

namespace UtilityPaymentJournal.Features.Complaints.Delete
{
    /// <summary>
    /// Команда на удаление карточки жалобы.
    /// </summary>
    /// <param name="Id">ID удаляемой записи.</param>
    public record DeleteComplaintCommand(long Id) : IRequest;
}
