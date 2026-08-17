using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Обработчик запроса на получение списка объектов недвижимости.
    /// </summary>
    public partial class GetResidencesListHandler : IRequestHandler<GetResidencesListQuery, GetResidencesListResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetResidencesListHandler> _logger;

        public GetResidencesListHandler(
            ApplicationDbContext context,
            ILogger<GetResidencesListHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetResidencesListResponse> Handle(GetResidencesListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllResidencesFromDb(_logger);

            List<Residence> entities = await _context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllResidencesFromDbCount(_logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
