using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.DTO.UtilityProviders;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUtilityProviderService
    {
        public Task<UtilityProviderDTO> CreateAsync(CreateUtilityProviderDTO createUtilityProviderDto);
        public Task DeleteAsync(long id);
        public Task<UtilityProviderDTO> EditAsync(long id, EditUtilityProviderDTO editUtilityProviderDto);
        public Task<IEnumerable<UtilityProviderDTO>> GetAllAsync();
    }
}
