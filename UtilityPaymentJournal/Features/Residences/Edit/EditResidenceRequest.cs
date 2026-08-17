namespace UtilityPaymentJournal.Features.Residences.Edit
{
    /// <summary>
    /// Запрос на редактировании жилого объекта.
    /// </summary>
    /// <param name="Address">Новый адрес жилого объекта.</param>
    public record EditResidenceRequest(string Address)
    {
        public EditResidenceCommand ToCommand(long id) =>
            new EditResidenceCommand(id, Address);
    }
}
