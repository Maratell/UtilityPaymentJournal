using UtilityPaymentJournal.DTO.Residences;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IResidenceService
    {
        public Task<ResidenceDTO> CreateAsync(CreateResidenceDTO createResidenceDto);
        public Task DeleteAsync(long id);
        public Task<ResidenceDTO> EditAsync(long id, EditResidenceDTO editResidenceDto);
        public Task<IEnumerable<ResidenceDTO>> GetAllAsync();
    }
}
