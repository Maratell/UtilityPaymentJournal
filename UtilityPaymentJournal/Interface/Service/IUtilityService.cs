using UtilityPaymentJournal.DTO.Utilities;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityService
    {
        public Task<UtilityDTO> CreateAsync(CreateUtilityDTO dto);
        public Task DeleteAsync(long id);
        public Task<UtilityDTO> EditAsync(long id, EditUtilityDTO dto);
        public Task<IEnumerable<UtilityDTO>> GetAllAsync();
    }
}
