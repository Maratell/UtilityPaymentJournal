using UtilityPaymentJournal.DTO.Admin;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Mapping
{
    public class UserMapper : IUserMapper
    {
        public CreateUserDTO ToDto(CreateUserViewModel createUserVM)
        {
            if (createUserVM == null)
                return null!;

            return new CreateUserDTO
            {
                UserName = createUserVM.UserName,
                FirstName = createUserVM.FirstName,
                LastName = createUserVM.LastName,
                Password = createUserVM.Password,
                Role = createUserVM.Role
            };
        }

        // Первый новый метод: собирает User из базы данных и роль в промежуточный DTO (используется в UserService)
        public UserDTO ToDto(User user, string roleName)
        {
            if (user == null)
                return null!;

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = roleName
            };
        }

        // Второй новый метод: превращает DTO в UI-модель ответа (используется в AdminApiController)
        public UserViewModel ToViewModel(UserDTO userDto)
        {
            if (userDto == null)
                return null!;

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
