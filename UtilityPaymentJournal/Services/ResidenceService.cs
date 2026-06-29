using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class ResidenceService : IResidenceService
    {
        private ApplicationDbContext _context;
        private IResidenceMapper _residenceMapper;

        public ResidenceService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper)
        {
            _context = context;
            _residenceMapper = residenceMapper;
        }

        public async Task<ResidenceDTO> CreateAsync(CreateResidenceDTO residenceDto)
        {
            Residence residence = _residenceMapper.ToEntity(residenceDto);

            await _context.Residences.AddAsync(residence);
            await _context.SaveChangesAsync();

            return _residenceMapper.ToDto(residence);
        }

        public async Task DeleteAsync(long id)
        {
            Residence residence = await FindByIdOrThrowAsync(id);

            _context.Residences.Remove(residence);
            await _context.SaveChangesAsync();
        }

        public async Task<ResidenceDTO> EditAsync(long id, EditResidenceDTO editResidenceDto)
        {
            Residence residence = await FindByIdOrThrowAsync(id);

            residence.Address = editResidenceDto.Address;
            residence.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _residenceMapper.ToDto(residence);
        }

        public async Task<IEnumerable<ResidenceDTO>> GetAllAsync()
        {
            List<Residence> result = await _context.Residences.ToListAsync();
            return result.Select(r => _residenceMapper.ToDto(r));
        }

        private async Task<Residence> FindByIdOrThrowAsync(long id)
        {
            Residence? residence = await _context.Residences.FirstOrDefaultAsync(r => r.Id == id);
            if (residence == null)
            {
                throw new KeyNotFoundException($"Жилой объект с ID {id} не найден.");
            }

            return residence;
        }
    }
}
