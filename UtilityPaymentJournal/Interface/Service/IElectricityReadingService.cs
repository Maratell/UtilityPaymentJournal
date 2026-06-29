using UtilityPaymentJournal.DTO.ElectricityReadings;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IElectricityReadingService
    {
        public Task<ElectricityReadingDTO> CreateAsync(CreateElectricityReadingDTO dto);
        public Task DeleteAsync(long id);
        public Task<ElectricityReadingDTO> EditAsync(long id, EditElectricityReadingDTO dto);
        public Task<IEnumerable<ElectricityReadingDTO>> GetAllAsync();
    }
}
