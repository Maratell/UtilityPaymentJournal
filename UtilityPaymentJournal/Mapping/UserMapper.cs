using UtilityPaymentJournal.DTOs.Admin;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Mapping
{
    public class UserMapper : IUserMapper
    {
        public CreateUserDto ToDto(CreateUserViewModel createUserViewModel)
        {
            ArgumentNullException.ThrowIfNull(createUserViewModel);

            return new CreateUserDto(
                UserName: createUserViewModel.UserName,
                FirstName: createUserViewModel.FirstName,
                LastName: createUserViewModel.LastName,
                Password: createUserViewModel.Password,
                Role: createUserViewModel.Role
            );
        }

        public UserDto ToDto(User user, string roleName)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

            return new UserDto(
                Id: user.Id,
                UserName: user.UserName ?? string.Empty,
                FirstName: user.FirstName,
                LastName: user.LastName,
                RoleName: roleName
            );
        }

        public UserViewModel ToViewModel(UserDto userDto)
        {
            ArgumentNullException.ThrowIfNull(userDto);

            return new UserViewModel
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                RoleName = userDto.RoleName
            };
        }
    }
}
