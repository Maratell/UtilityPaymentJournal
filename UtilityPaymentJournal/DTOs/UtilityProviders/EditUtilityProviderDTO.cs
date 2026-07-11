namespace UtilityPaymentJournal.DTOs.UtilityProviders
{
    /// <summary>
    /// ДТО для редактирования существующего поставщика коммунальных услуг.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика в бд</param>
    /// <param name="Name">Новое наименование поставщика услуг</param>
    public record EditUtilityProviderDto(long Id, string Name);
}
