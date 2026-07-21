namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    /// <summary>
    /// ДТО для создания нового поставщика коммунальных услуг.
    /// </summary>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record CreateUtilityProviderDto(
        string Name
    );
}
