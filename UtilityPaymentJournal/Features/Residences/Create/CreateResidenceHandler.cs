using MediatR;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// Обработчик команды создания объекта недвижимости.
    /// Инкапсулирует в себе всю бизнес-логику и запись в базу данных PostgreSQL для этой фичи.
    /// </summary>
    public partial class CreateResidenceHandler(
        ApplicationDbContext context,
        ILogger<CreateResidenceHandler> logger) : IRequestHandler<CreateResidenceCommand, CreateResidenceResponse>
    {
        public async Task<CreateResidenceResponse> Handle(CreateResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceCreationRequested(logger, command.Address);
            Residence entity = command.ToEntity();

            // Используем синхронный Add, так как операция происходит в памяти
            context.Residences.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogResidenceCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
