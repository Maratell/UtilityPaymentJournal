using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.DTOs.Account;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IAccountMapper
    {
        // Преобразование ViewModel (запрос от фронтенда) во входное Dto 
        SignInDto ToDto(SignInRequestViewModel viewModel);

        // Преобразование DTO ответа из сервиса во ViewModel 
        AuthenticationResultViewModel ToViewModel(AuthenticationResultDTO dto);

        // Преобразование инфраструктурного результата Identity в Dto 
        AuthenticationResultDTO ToDto(SignInResult signInResult);
    }
}
