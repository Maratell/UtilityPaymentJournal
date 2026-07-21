namespace UtilityPaymentJournal.Features.Residences.Commands
{
    /// <summary>
    /// ДТО для редактирования существующего объекта недвижимости.
    /// </summary>
    /// <param name="Address">Новый полный адрес объекта недвижимости</param>
    public record EditResidenceDto(
        string Address
    );
}
