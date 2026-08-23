namespace UtilityPaymentJournal.Features.Utilities.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одной услуге.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги, подтянутый из базы данных.</param>
    /// <param name="Name">Наименование коммунальной услуги (например, "Водоснабжение", "Отопление")</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons для визуализации в интерфейсе</param>
    /// <param name="IsActive">Статус активности услуги (доступна ли для выбора в новых операциях)</param>
    public record GetUtilityByIdResponse(
        long Id,
        string Name,
        string IconClass,
        bool IsActive
    );
}
