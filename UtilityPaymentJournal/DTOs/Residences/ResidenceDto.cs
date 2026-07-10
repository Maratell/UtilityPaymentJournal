namespace UtilityPaymentJournal.DTOs.Residences
{
    /// <summary>
    /// ДТО для возврата данных об объекте недвижимости (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта</param>
    /// <param name="Address">Адрес объекта</param>
    public record ResidenceDto(long Id, string Address);
}
