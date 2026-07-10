namespace UtilityPaymentJournal.DTOs.Utilities
{
    /// <summary>
    /// ДТО для редактирования существующей коммунальной услуги.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги в бд</param>
    /// <param name="Name">Новое наименование коммунальной услуги</param>
    public record EditUtilityDto(long Id, string Name);
}
