namespace UtilityPaymentJournal.Features.UtilityProviders.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одном поставщике услуг.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика услуг, подтянутый из базы данных.</param>
    /// <param name="Address">Полный текстовый адрес поставщика услуг.</param>
    public record GetUtilityProviderByIdResponse(
        long Id,
        string Name
    );
}
