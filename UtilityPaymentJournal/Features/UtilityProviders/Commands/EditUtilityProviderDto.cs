namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    /// <summary>
    /// ДТО для редактирования существующего поставщика коммунальных услуг.
    /// </summary>
    /// <param name="Name">Новое наименование поставщика услуг</param>
    public record EditUtilityProviderDto(
        string Name
    );
}
