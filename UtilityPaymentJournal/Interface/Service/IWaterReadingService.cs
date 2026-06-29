using UtilityPaymentJournal.DTO.WaterReadings;

namespace WaterReadingPaymentJournal.Interface.Service
{
    public interface IWaterReadingService
    {
        public Task<WaterReadingDTO> CreateAsync(CreateWaterReadingDTO createWaterReadingDto);
        public Task DeleteAsync(long id);
        public Task<WaterReadingDTO> EditAsync(long id, EditWaterReadingDTO editWaterReadingDto);
        public Task<IEnumerable<WaterReadingDTO>> GetAllAsync();
    }
}
