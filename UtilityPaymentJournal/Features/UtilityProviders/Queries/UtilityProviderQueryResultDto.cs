namespace UtilityPaymentJournal.Features.UtilityProviders.Queries
{
    /// <summary>
    /// ДТО результата запроса данных поставщика коммунальных услуг.
    /// Используется для передачи полной информации клиенту в UI (GetById/GetAll).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика</param>
    /// <param name="Name">Наименование поставщика услуг, подтянутое из БД</param>
    public record UtilityProviderQueryResultDto(
        long Id,
        string Name
    );
}
