namespace UtilityPaymentJournal.Features.Residences.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одном объекте недвижимости.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости, подтянутый из базы данных.</param>
    /// <param name="Address">Полный текстовый адрес объекта недвижимости.</param>
    public record GetResidenceByIdResponse(
        long Id,
        string Address
    );
}
