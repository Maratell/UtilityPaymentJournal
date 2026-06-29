using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.ElectricityReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class ElectricityReadingService : IElectricityReadingService
    {
        private ApplicationDbContext _context;
        private IElectricityReadingMapper _electricityReadingMapper;

        public ElectricityReadingService(
            ApplicationDbContext context,
            IElectricityReadingMapper electricityReadingMapper)
        {
            _context = context;
            _electricityReadingMapper = electricityReadingMapper;
        }

        public async Task<ElectricityReadingDTO> CreateAsync(CreateElectricityReadingDTO createElectricityReadingDto)
        {
            ElectricityReading electricityReading = _electricityReadingMapper.ToEntity(createElectricityReadingDto);

            await _context.ElectricityReadings.AddAsync(electricityReading);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }

            ElectricityReading? savedElectricityReading = await GetElectricityReadingWithDetailsAsync(electricityReading.Id);
            if (savedElectricityReading == null)
            {
                throw new KeyNotFoundException("Запись не найдена");
            }

            return _electricityReadingMapper.ToDto(savedElectricityReading);
        }

        public async Task DeleteAsync(long id)
        {
            ElectricityReading electricityReading = await FindByIdOrThrowAsync(id);

            _context.ElectricityReadings.Remove(electricityReading);
            await _context.SaveChangesAsync();
        }

        public async Task<ElectricityReadingDTO> EditAsync(long id, EditElectricityReadingDTO editElectricityReadingDto)
        {
            ElectricityReading electricityReading = await FindByIdOrThrowAsync(id);

            _electricityReadingMapper.UpdateEntity(editElectricityReadingDto, electricityReading);
            await _context.SaveChangesAsync();

            ElectricityReading? editedElectricityReading = await GetElectricityReadingWithDetailsAsync(electricityReading.Id);
            return _electricityReadingMapper.ToDto(electricityReading);
        }

        public async Task<IEnumerable<ElectricityReadingDTO>> GetAllAsync()
        {
            IEnumerable<ElectricityReading> electricityReadings = await GetElectricityReadingsWithDetailsAsync();

            return electricityReadings.Select(w => _electricityReadingMapper.ToDto(w));
        }

        private async Task<IEnumerable<ElectricityReading>> GetElectricityReadingsWithDetailsAsync()
        {
            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            return await _context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .ToListAsync();
        }

        private async Task<ElectricityReading?> GetElectricityReadingWithDetailsAsync(long id)
        {
            ElectricityReading? entity = await _context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return entity;
        }

        private async Task<ElectricityReading> FindByIdOrThrowAsync(long id)
        {
            ElectricityReading? electricityReading = await _context.ElectricityReadings.FirstOrDefaultAsync(r => r.Id == id);
            if (electricityReading == null)
            {
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return electricityReading;
        }
    }
}
