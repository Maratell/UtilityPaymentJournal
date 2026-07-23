using UtilityPaymentJournal.Features.Account.Commands;
using UtilityPaymentJournal.Features.Account.Models;
using UtilityPaymentJournal.Features.Account.Queries;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    public class AccountMapper : IAccountMapper
    {
        public SignInDto ToSignInDto(SignInRequestViewModel signInViewModel)
        {
            ArgumentNullException.ThrowIfNull(signInViewModel);

            return new SignInDto(
                UserName: signInViewModel.UserName,
                Password: signInViewModel.Password, 
                IsPersistent: signInViewModel.IsPersistent
            );
        }

        public UserSignedInViewModel ToSignedInViewModel(AuthenticationCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UserSignedInViewModel
            {
                IsSuccess = dto.IsSuccess,
                Status = dto.Status,
                ErrorMessage = dto.ErrorMessage
                // Свойство RedirectUrl намеренно оставляем пустым, 
                // так как за генерацию путей отвечает исключительно Контроллер
            };
        }

        public CurrentUserDetailsViewModel ToDetailsViewModel(CurrentUserQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new CurrentUserDetailsViewModel
            {
                Id = dto.Id,
                UserName = dto.UserName
            };
        }
    }
}
