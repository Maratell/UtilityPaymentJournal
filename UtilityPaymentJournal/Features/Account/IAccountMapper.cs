using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    public interface IAccountMapper
    {
        SignInDto ToSignInDto(SignInRequestViewModel signInRequestViewModel);
        AuthenticationResultViewModel ToViewModel(AuthenticationResultDto authenticationResultDto);
    }
}
