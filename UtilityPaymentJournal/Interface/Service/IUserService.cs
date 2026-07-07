using UtilityPaymentJournal.DTO.Admin;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken cancellationToken = default);
        Task<UserDTO?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
