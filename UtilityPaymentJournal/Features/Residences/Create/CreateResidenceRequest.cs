namespace UtilityPaymentJournal.Features.Residences.Create
{
    /// <summary>
    /// Запрос на создание жилого объекта.
    /// </summary>
    /// <param name="Address">Адрес жилого объекта.</param>
    public record CreateResidenceRequest(string Address)
    {
        public CreateResidenceCommand ToCommand() =>
            new CreateResidenceCommand(Address);
    }
}
