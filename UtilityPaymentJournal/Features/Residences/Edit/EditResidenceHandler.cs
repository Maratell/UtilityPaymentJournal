using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Обработчик команды редактирования объекта недвижимости.
    /// </summary>
    public partial class EditResidenceHandler : IRequestHandler<EditResidenceCommand, EditResidenceResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditResidenceHandler> _logger;

        public EditResidenceHandler(
            ApplicationDbContext context,
            ILogger<EditResidenceHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EditResidenceResponse> Handle(EditResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceUpdateRequested(_logger, command.Id, command.Address);

            Residence? entity = await _context.Residences
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogResidenceNotFoundInDb(_logger, command.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {command.Id} не найдено в базе данных.");
            }

            command.UpdateEntity(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceUpdatedInDb(_logger, command.Id);
            return entity.ToResponse();
        }
    }
}
