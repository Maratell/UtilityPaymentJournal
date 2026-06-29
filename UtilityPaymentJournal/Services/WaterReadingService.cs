using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Service;

namespace WaterReadingPaymentJournal.Services
{
    public class WaterReadingService : IWaterReadingService
    {
        private ApplicationDbContext _context;
        private IWaterReadingMapper _waterReadingMapper;

        public WaterReadingService(
            ApplicationDbContext context,
            IWaterReadingMapper waterReadingMapper)
        {
            _context = context;
            _waterReadingMapper = waterReadingMapper;
        }

        public async Task<WaterReadingDTO> CreateAsync(CreateWaterReadingDTO createWaterReadingDto)
        {
            WaterReading waterReading = _waterReadingMapper.ToEntity(createWaterReadingDto);

            await _context.WaterReadings.AddAsync(waterReading);
            await _context.SaveChangesAsync();

            WaterReading? savedWaterReading = await GetWaterReadingWithDetailsAsync(waterReading.Id);
            if (savedWaterReading == null)
            {
                throw new KeyNotFoundException("Запись не найдена");
            }

            return _waterReadingMapper.ToDto(savedWaterReading);
        }

        public async Task DeleteAsync(long id)
        {
            WaterReading waterReading = await FindByIdOrThrowAsync(id);

            _context.WaterReadings.Remove(waterReading);
            await _context.SaveChangesAsync();
        }

        public async Task<WaterReadingDTO> EditAsync(long id, EditWaterReadingDTO editWaterReadingDto)
        {
            WaterReading waterReading = await FindByIdOrThrowAsync(id);

            _waterReadingMapper.UpdateEntity(editWaterReadingDto, waterReading);
            await _context.SaveChangesAsync();

            WaterReading? editedWaterReading = await GetWaterReadingWithDetailsAsync(waterReading.Id);
            return _waterReadingMapper.ToDto(waterReading);
        }

        public async Task<IEnumerable<WaterReadingDTO>> GetAllAsync()
        {
            IEnumerable<WaterReading> waterReadings = await GetWaterReadingsWithDetailsAsync();

            return waterReadings.Select(w => _waterReadingMapper.ToDto(w));
        }

        private async Task<IEnumerable<WaterReading>> GetWaterReadingsWithDetailsAsync()
        {
            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            return await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .ToListAsync();
        }

        private async Task<WaterReading?> GetWaterReadingWithDetailsAsync(long id)
        {
            WaterReading? entity = await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return entity;
        }

        private async Task<WaterReading> FindByIdOrThrowAsync(long id)
        {
            WaterReading? waterReading = await _context.WaterReadings.FirstOrDefaultAsync(r => r.Id == id);
            if (waterReading == null)
            {
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return waterReading;
        }
    }
}
