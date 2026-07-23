using UtilityPaymentJournal.Features.Account.Commands;
using UtilityPaymentJournal.Features.Account.Models;
using UtilityPaymentJournal.Features.Account.Queries;
using UtilityPaymentJournal.Models.Authentication;

namespace UtilityPaymentJournal.Features.Account
{
    /// <summary>
    /// Интерфейс маппера для преобразования моделей данных учетной записи пользователя между слоями.
    /// </summary>
    public interface IAccountMapper
    {
        /// <summary>
        /// Преобразовать входящую модель запроса авторизации во входной ДТО бизнес-логики.
        /// </summary>
        SignInDto ToSignInDto(SignInRequestViewModel signInViewModel);
        /// <summary>
        /// Преобразовать ДТО результата команды входа в модель ответа API аутентификации (для POST).
        /// </summary>
        UserSignedInViewModel ToSignedInViewModel(AuthenticationCommandResultDto dto);
        /// <summary>
        /// Преобразовать ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        CurrentUserDetailsViewModel ToDetailsViewModel(CurrentUserQueryResultDto dto);
    }
}
