namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданного объекта поставщика услуг с присвоенным идентификатором.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика услуг, сгенерированный базой данных.</param>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record CreateUtilityProviderResponse
    (
        long Id,
        string Name
    );
}
