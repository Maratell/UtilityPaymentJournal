namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// ДТО результата запроса данных коммунальной услуги.
    /// Используется для передачи полной информации клиенту в UI (GetById/GetAll).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор коммунальной услуги</param>
    /// <param name="Name">Наименование коммунальной услуги (например, "Водоснабжение", "Отопление")</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons для визуализации в интерфейсе</param>
    /// <param name="IsActive">Статус активности услуги (доступна ли для выбора в новых операциях)</param>
    public record UtilityQueryResultDto(
        long Id,
        string Name,
        string IconClass,
        bool IsActive
    );
}
