namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    /// <summary>
    /// Запрос на создание поставщика услуг
    /// </summary>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record CreateUtilityProviderRequest(string Name)
    {
        public CreateUtilityProviderCommand ToCommand()
            => new CreateUtilityProviderCommand(Name);
    }
}
