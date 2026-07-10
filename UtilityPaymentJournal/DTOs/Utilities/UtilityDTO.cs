namespace UtilityPaymentJournal.DTOs.Utilities
{
    /// <summary>
    /// ДТО для возврата данных о коммунальной услуге (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги</param>
    /// <param name="Name">Наименование коммунальной услуги</param>
    public record UtilityDto(long Id, string Name);
}
