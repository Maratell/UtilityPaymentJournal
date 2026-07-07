using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTO.Admin
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
