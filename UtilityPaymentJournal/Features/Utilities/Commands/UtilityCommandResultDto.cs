namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    /// <summary>
    /// ДТО для возврата данных о коммунальной услуге после выполнения команд (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги</param>
    /// <param name="Name">Наименование коммунальной услуги</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons</param>
    /// <param name="IsActive">Флаг активности услуги</param>
    public record UtilityCommandResultDto(
        long Id,
        string Name,
        string IconClass,
        bool IsActive
    );
}
