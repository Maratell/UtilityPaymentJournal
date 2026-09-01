using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Create
{
    public partial class CreateUtilityHandler(
        IApplicationDbContext context,
        ILogger<CreateUtilityHandler> logger) : IRequestHandler<CreateUtilityCommand, CreateUtilityResponse>
    {
        public async Task<CreateUtilityResponse> Handle(CreateUtilityCommand command, CancellationToken cancellationToken)
        {
            LogUtilityCreationRequested(logger, command.Name);
            Utility entity = command.ToEntity();

            // Используем синхронный Add, так как операция добавления в DbSet происходит в памяти
            context.Utilities.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogUtilityCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
