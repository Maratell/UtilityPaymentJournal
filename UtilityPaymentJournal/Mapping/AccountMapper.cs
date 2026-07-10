using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTOs.Account;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Mapping
{
    public class AccountMapper : IAccountMapper
    {
        /// <summary>
        /// Маппинг входных данных фронтенда в DTO для бизнес-логики
        /// </summary>
        public SignInDto ToDto(SignInRequestViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            return new SignInDto
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                IsPersistent = viewModel.IsPersistent
            };
        }

        /// <summary>
        /// Маппинг результата бизнес-логики во ViewModel ответа (без Url)
        /// </summary>
        public AuthenticationResultViewModel ToViewModel(AuthenticationResultDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new AuthenticationResultViewModel
            {
                IsSuccess = dto.IsSuccess,
                Status = dto.Status,
                ErrorMessage = dto.ErrorMessage
                // Свойство RedirectUrl намеренно оставляем пустым, 
                // так как за генерацию путей отвечает исключительно Контроллер
            };
        }

        /// <summary>
        /// Перевод инфраструктурного SignInResult от Identity в понятный DTO для сервиса
        /// </summary>
        public AuthenticationResultDTO ToDto(SignInResult signInResult)
        {
            if (signInResult == null)
                throw new ArgumentNullException(nameof(signInResult));

            // Успешная аутентификация
            if (signInResult.Succeeded)
            {
                return new AuthenticationResultDTO
                {
                    IsSuccess = true,
                    Status = SignInResultStatus.Success
                };
            }

            // Во всех остальных случаях (неверный пароль, отсутствие пользователя и т.д.)
            // Трактуем как неверные учетные данные, так как Email в системе не задействован
            return new AuthenticationResultDTO
            {
                IsSuccess = false,
                Status = SignInResultStatus.InvalidCredentials,
                ErrorMessage = "Неверный логин или пароль."
            };
        }
    }
}
