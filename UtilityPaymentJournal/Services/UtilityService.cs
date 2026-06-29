using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityMapper _utilityMapper;

        public UtilityService(
            ApplicationDbContext context,
            IUtilityMapper uilityMapper)
        {
            _context = context;
            _utilityMapper = uilityMapper;
        }

        public async Task<UtilityDTO> CreateAsync(CreateUtilityDTO uilityDto)
        {
            Utility entity = _utilityMapper.ToEntity(uilityDto);

            await _context.Utilities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _utilityMapper.ToDto(entity);
        }

        public async Task DeleteAsync(long id)
        {
            Utility entity = await FindByIdOrThrowAsync(id);

            _context.Utilities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<UtilityDTO> EditAsync(long id, EditUtilityDTO editUtilityDto)
        {
            Utility entity = await FindByIdOrThrowAsync(id);

            entity.Name = editUtilityDto.Name;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _utilityMapper.ToDto(entity);
        }

        public async Task<IEnumerable<UtilityDTO>> GetAllAsync()
        {
            List<Utility> result = await _context.Utilities.ToListAsync();

            return result.Select(r => _utilityMapper.ToDto(r));
        }

        private async Task<Utility> FindByIdOrThrowAsync(long id)
        {
            Utility? entity = await _context.Utilities.FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Услуга с ID {id} не найден.");
            }

            return entity;
        }
    }
}
