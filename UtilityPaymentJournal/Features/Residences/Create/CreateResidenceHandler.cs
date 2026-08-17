using MediatR;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// Обработчик команды создания объекта недвижимости.
    /// Инкапсулирует в себе всю бизнес-логику и запись в базу данных PostgreSQL для этой фичи.
    /// </summary>
    public partial class CreateResidenceHandler : IRequestHandler<CreateResidenceCommand, CreateResidenceResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateResidenceHandler> _logger;

        public CreateResidenceHandler(
            ApplicationDbContext context,
            ILogger<CreateResidenceHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateResidenceResponse> Handle(CreateResidenceCommand command, CancellationToken cancellationToken)
        {
            LogResidenceCreationRequested(_logger, command.Address);
            Residence entity = command.ToEntity();

            // Используем синхронный Add, так как операция происходит в памяти
            _context.Residences.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceCreatedInDb(_logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
