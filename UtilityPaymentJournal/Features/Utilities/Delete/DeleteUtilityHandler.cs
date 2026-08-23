using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Utilities.Delete
{
    /// <summary>
    /// Обработчик команды удаления услуги.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteUtilityHandler(
            ApplicationDbContext context,
            ILogger<DeleteUtilityHandler> logger) : IRequestHandler<DeleteUtilityCommand>
    {
        public async Task Handle(DeleteUtilityCommand command, CancellationToken cancellationToken)
        {
            LogUtilityDeletionRequested(logger, command.Id);

            // Высокопроизводительное удаление: EF Core сразу генерирует SQL-запрос DELETE без загрузки сущности в память
            int deletedRowsCount = await context.Utilities
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogUtilityNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Коммунальная услуга с ID {command.Id} не найдена.");
            }

            LogUtilityDeletedFromDb(logger, command.Id);
        }
    }
}
