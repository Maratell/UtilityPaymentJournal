namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Ответ API, содержащий отредактированные данные объекта недвижимости.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости.</param>
    /// <param name="Address">Обновленный адрес объекта недвижимости.</param>
    public record EditResidenceResponse(long Id, string Address);
}
