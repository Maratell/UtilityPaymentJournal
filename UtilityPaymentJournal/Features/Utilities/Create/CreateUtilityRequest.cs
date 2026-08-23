namespace UtilityPaymentJournal.Features.Utilities.Create
{
    /// <summary>
    /// Запрос на создание поставщика услуг
    /// </summary>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record CreateUtilityRequest(
        string Name,
        string IconClass,
        bool IsActive = true
    )
    {
        public CreateUtilityCommand ToCommand() =>
            new CreateUtilityCommand(
                Name,
                IconClass,
                IsActive
            );
    }
}
