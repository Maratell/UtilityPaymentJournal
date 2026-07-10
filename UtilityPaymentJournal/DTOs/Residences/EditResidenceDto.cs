namespace UtilityPaymentJournal.DTOs.Residences
{
    /// <summary>
    /// ДТО для редактирования существующего объекта недвижимости.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта в бд</param>
    /// <param name="Address">Новый адрес объекта</param>
    public record EditResidenceDto(long Id, string Address);
}
