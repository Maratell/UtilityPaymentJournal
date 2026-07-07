using UtilityPaymentJournal.DTO.Admin;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Models.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IUserMapper
    {
        CreateUserDTO ToDto(CreateUserViewModel createUserVM);
        UserDTO ToDto(User user, string roleName);
        UserViewModel ToViewModel(UserDTO userDto);
    }
}
