using UtilityPaymentJournal.DTOs.Admin;

namespace UtilityPaymentJournal.Interface.Service
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
