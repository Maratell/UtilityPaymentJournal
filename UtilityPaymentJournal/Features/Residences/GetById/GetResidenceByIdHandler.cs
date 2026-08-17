using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Features.Residences.Create;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей объекта недвижимости.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetResidenceByIdHandler : IRequestHandler<GetResidenceByIdQuery, GetResidenceByIdResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetResidenceByIdHandler> _logger;

        public GetResidenceByIdHandler(
            ApplicationDbContext context,
            ILogger<GetResidenceByIdHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetResidenceByIdResponse> Handle(GetResidenceByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingResidenceByIdFromDb(_logger, query.Id);

            Residence? entity = await _context.Residences
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogResidenceNotFoundInDb(_logger, query.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {query.Id} не найдено.");
            }

            return entity.ToResponse();
        }
    }
}
