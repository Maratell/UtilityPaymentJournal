using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Complaints.Delete
{
    /// <summary>
    /// Обработчик команды удаления карточки жалобы.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteComplaintHandler(
            ApplicationDbContext context,
            ILogger<DeleteComplaintHandler> logger) : IRequestHandler<DeleteComplaintCommand>
    {
        public async Task Handle(DeleteComplaintCommand command, CancellationToken cancellationToken)
        {
            LogComplaintDeletionRequested(logger, command.Id);

            int deletedRowsCount = await context.Complaints
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogComplaintNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Жалоба с ID {command.Id} не найдена.");
            }

            LogComplaintDeletedFromDb(logger, command.Id);
        }
    }
}
