namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    /// <summary>
    /// Ответ API, содержащий отредактированные данные поставщика услуг.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика услуг.</param>
    /// <param name="Name">Новое наименование поставщика услуг.</param>
    public record EditUtilityProviderResponse(long Id, string Name);
}
