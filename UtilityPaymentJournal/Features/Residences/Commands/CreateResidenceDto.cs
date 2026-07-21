namespace UtilityPaymentJournal.Features.Residences.Commands
{
    /// <summary>
    /// ДТО для создания нового объекта недвижимости.
    /// </summary>
    /// <param name="Address">Полный адрес объекта недвижимости</param>
    public record CreateResidenceDto(
        string Address
    );
}
