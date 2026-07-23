namespace UtilityPaymentJournal.Features.Account.Models
{
    /// <summary>
    /// Развернутая модель представления данных текущего пользователя для отображения на UI (ответ на GET).
    /// </summary>
    public class CurrentUserDetailsViewModel
    {
        /// <summary>
        /// Уникальный строковый идентификатор записи пользователя в БД.
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Имя пользователя (логин) в системе.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}
