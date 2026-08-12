using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Users.Commands
{
    /// <summary>
    /// ДТО для возврата данных о пользователе после выполнения команды записи (ответ бизнес-логики).
    /// Подтверждает успешное изменение состояния системы (W) и возвращает полный плоский результат мутации.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор (GUID) созданного пользователя, сгенерированный базой данных</param>
    /// <param name="UserName">Уникальное имя пользователя (логин), под которым он был зарегистрирован</param>
    /// <param name="FirstName">Имя созданного пользователя</param>
    /// <param name="LastName">Фамилия созданного пользователя</param>
    /// <param name="RoleName">Название системной роли, которая была успешно присвоена пользователю</param>
    public record UserCommandResultDto(
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string RoleName
    );
}