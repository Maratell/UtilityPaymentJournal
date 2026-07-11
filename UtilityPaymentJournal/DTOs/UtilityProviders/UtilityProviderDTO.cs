namespace UtilityPaymentJournal.DTOs.UtilityProviders
{
    /// <summary>
    /// ДТО для возврата данных о поставщике коммунальных услуг (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика</param>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record UtilityProviderDto(long Id, string Name);
}