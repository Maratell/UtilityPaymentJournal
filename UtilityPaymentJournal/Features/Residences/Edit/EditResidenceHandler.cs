using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Обработчик команды редактирования объекта недвижимости.
    /// </summary>
    public partial class EditResidenceHandler(
            IApplicationDbContext context,
            ILogger<EditResidenceHandler> logger) : IRequestHandler<EditResidenceCommand, EditResidenceResponse>
    {
        public async Task<EditResidenceResponse> Handle(EditResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceUpdateRequested(logger, command.Id, command.Address);

            Residence? entity = await context.Residences
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogResidenceNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {command.Id} не найдено в базе данных.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogResidenceUpdatedInDb(logger, command.Id);
            return entity.ToResponse();
        }
    }
}
