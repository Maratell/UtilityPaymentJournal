namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданного объекта недвижимости с присвоенным идентификатором.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости, сгенерированный базой данных.</param>
    /// <param name="Address">Полный адрес объекта недвижимости.</param>
    public record CreateResidenceResponse(
        long Id,
        string Address
    );
}
