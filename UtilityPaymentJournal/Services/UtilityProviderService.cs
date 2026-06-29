using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Services
{
    public class UtilityProviderService : IUtilityProviderService
    {
        private ApplicationDbContext _context;
        private IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProviderService(
            ApplicationDbContext context,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _context = context;
            _utilityProviderMapper = utilityProviderMapper;
        }

        public async Task<UtilityProviderDTO> CreateAsync(CreateUtilityProviderDTO utilityProviderDto)
        {
            UtilityProvider utilityProvider = _utilityProviderMapper.ToEntity(utilityProviderDto);

            await _context.UtilityProviders.AddAsync(utilityProvider);
            await _context.SaveChangesAsync();

            return _utilityProviderMapper.ToDto(utilityProvider);
        }

        public async Task DeleteAsync(long id)
        {
            UtilityProvider utility = await FindByIdOrThrowAsync(id);

            _context.UtilityProviders.Remove(utility);
            await _context.SaveChangesAsync();
        }

        public async Task<UtilityProviderDTO> EditAsync(long id, EditUtilityProviderDTO editUtilityProviderDto)
        {
            UtilityProvider utilityProvider = await FindByIdOrThrowAsync(id);

            utilityProvider.Name = editUtilityProviderDto.Name;
            utilityProvider.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _utilityProviderMapper.ToDto(utilityProvider);
        }

        public async Task<IEnumerable<UtilityProviderDTO>> GetAllAsync()
        {
            List<UtilityProvider> result = await _context.UtilityProviders.ToListAsync();

            return result.Select(r => _utilityProviderMapper.ToDto(r));
        }

        private async Task<UtilityProvider> FindByIdOrThrowAsync(long id)
        {
            UtilityProvider? utilityProvider = await _context.UtilityProviders.FirstOrDefaultAsync(r => r.Id == id);
            if (utilityProvider == null)
            {
                throw new KeyNotFoundException($"Поставщик услуг с ID {id} не найден.");
            }

            return utilityProvider;
        }
    }
}
