using UtilityPaymentJournal.DTOs.Admin;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Models.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IUserMapper
    {
        CreateUserDto ToDto(CreateUserViewModel createUserViewModel);
        UserDto ToDto(User user, string roleName);
        UserViewModel ToViewModel(UserDto userDto);
    }
}
