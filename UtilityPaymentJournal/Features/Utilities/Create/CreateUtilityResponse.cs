namespace UtilityPaymentJournal.Features.Utilities.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданной услуги с присвоенным идентификатором.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги, сгенерированный базой данных.</param>
    /// <param name="Name">Наименование коммунальной услуги</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons</param>
    /// <param name="IsActive">Флаг активности услуги</param>
    public record CreateUtilityResponse
    (
        long Id,
        string Name,
        string IconClass,
        bool IsActive
    );
}
