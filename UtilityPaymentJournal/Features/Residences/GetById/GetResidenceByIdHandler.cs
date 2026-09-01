using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей объекта недвижимости.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetResidenceByIdHandler(
            IApplicationDbContext context,
            ILogger<GetResidenceByIdHandler> logger) : IRequestHandler<GetResidenceByIdQuery, GetResidenceByIdResponse>
    {
        public async Task<GetResidenceByIdResponse> Handle(GetResidenceByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingResidenceByIdFromDb(logger, query.Id);

            Residence? entity = await context.Residences
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogResidenceNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {query.Id} не найдено.");
            }

            return entity.ToResponse();
        }
    }
}
