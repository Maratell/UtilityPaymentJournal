using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTOs.Account;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IAccountMapper
    {
        SignInDto ToDto(SignInRequestViewModel signInRequestViewModel);
        AuthenticationResultViewModel ToViewModel(AuthenticationResultDto authenticationResultDto);
        AuthenticationResultDto ToDto(bool isSuccess, SignInResultStatus status, string? errorMessage = null);
    }
}
