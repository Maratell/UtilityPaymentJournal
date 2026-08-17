using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.Residences.Delete
{
    /// <summary>
    /// Обработчик команды удаления объекта недвижимости.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteResidenceHandler : IRequestHandler<DeleteResidenceCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteResidenceHandler> _logger;

        public DeleteResidenceHandler(
            ApplicationDbContext context,
            ILogger<DeleteResidenceHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DeleteResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceDeletionRequested(_logger, command.Id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
            int deletedRowsCount = await _context.Residences
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogResidenceNotFoundInDb(_logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика электроэнергии с ID {command.Id} не найдено.");
            }

            LogResidenceDeletedFromDb(_logger, command.Id);
        }
    }
}
