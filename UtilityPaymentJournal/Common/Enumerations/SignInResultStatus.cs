namespace UtilityPaymentJournal.Common.Enumerations
{
    /// <summary>
    /// Статус результата попытки входа пользователя в систему.
    /// Переводит внутренние состояния Identity в безопасные бизнес-статусы API.
    /// </summary>
    public enum SignInResultStatus
    {
        /// <summary>
        /// Аутентификация выполнена успешно, сессия создана.
        /// </summary>
        Success,

        /// <summary>
        /// Указаны неверные учетные данные (логин или пароль).
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// Учетная запись временно заблокирована из-за частых неверных попыток входа.
        /// </summary>
        LockedOut,

        /// <summary>
        /// Вход запрещен внутренними правилами системы (например, не подтвержден Email или аккаунт деактивирован).
        /// </summary>
        NotAllowed
    }
}
