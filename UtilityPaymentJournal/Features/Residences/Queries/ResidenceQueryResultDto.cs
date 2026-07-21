namespace UtilityPaymentJournal.Features.Residences.Queries
{
    /// <summary>
    /// ДТО результата запроса данных объекта недвижимости.
    /// Используется для передачи полной информации клиенту в UI (GetById/GetAll).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости</param>
    /// <param name="Address">Полный текстовый адрес объекта недвижимости, подтянутый из БД</param>
    public record ResidenceQueryResultDto(
        long Id,
        string Address
    );
}
