using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Residences.Delete
{
    /// <summary>
    /// Обработчик команды удаления объекта недвижимости.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteResidenceHandler(
            ApplicationDbContext context,
            ILogger<DeleteResidenceHandler> logger) : IRequestHandler<DeleteResidenceCommand>
    {
        public async Task Handle(DeleteResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceDeletionRequested(logger, command.Id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM Residences WHERE Id = @id
            int deletedRowsCount = await context.Residences
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogResidenceNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Жилой объект с ID {command.Id} не найден.");
            }

            LogResidenceDeletedFromDb(logger, command.Id);
        }
    }
}
