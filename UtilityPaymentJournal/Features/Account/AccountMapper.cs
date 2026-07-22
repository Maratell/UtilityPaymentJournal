using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    public class AccountMapper : IAccountMapper
    {
        /// <summary>
        /// Маппинг входных данных фронтенда в DTO для бизнес-логики
        /// </summary>
        public SignInDto ToSignInDto(SignInRequestViewModel signInRequestViewModel)
        {
            ArgumentNullException.ThrowIfNull(signInRequestViewModel);

            return new SignInDto(
                UserName: signInRequestViewModel.UserName,
                Password: signInRequestViewModel.Password,
                IsPersistent: signInRequestViewModel.IsPersistent
            );
        }

        /// <summary>
        /// Маппинг результата бизнес-логики во ViewModel ответа (без Url)
        /// </summary>
        public AuthenticationResultViewModel ToViewModel(AuthenticationResultDto authenticationResultDto)
        {
            ArgumentNullException.ThrowIfNull(authenticationResultDto);

            return new AuthenticationResultViewModel
            {
                IsSuccess = authenticationResultDto.IsSuccess,
                Status = authenticationResultDto.Status,
                ErrorMessage = authenticationResultDto.ErrorMessage
                // Свойство RedirectUrl намеренно оставляем пустым, 
                // так как за генерацию путей отвечает исключительно Контроллер
            };
        }
    }
}
