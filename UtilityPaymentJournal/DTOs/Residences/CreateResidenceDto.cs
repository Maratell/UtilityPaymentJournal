namespace UtilityPaymentJournal.DTOs.Residences
{
    /// <summary>
    /// ДТО для создания нового объекта недвижимости.
    /// </summary>
    /// <param name="Address">Полный адрес объекта</param>
    public record CreateResidenceDto(string Address);
}
