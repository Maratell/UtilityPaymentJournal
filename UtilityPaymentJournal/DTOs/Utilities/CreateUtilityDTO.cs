namespace UtilityPaymentJournal.DTOs.Utilities
{
    /// <summary>
    /// ДТО для создания новой коммунальной услуги.
    /// </summary>
    /// <param name="Name">Наименование коммунальной услуги (например, "Водоснабжение", "Электроэнергия").</param>
    public record CreateUtilityDto(string Name);
}
