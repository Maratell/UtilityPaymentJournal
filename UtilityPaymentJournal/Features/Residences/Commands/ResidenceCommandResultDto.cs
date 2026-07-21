namespace UtilityPaymentJournal.Features.Residences.Commands
{
    /// <summary>
    /// ДТО для возврата данных об объекте недвижимости после выполнения команды записи (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор объекта недвижимости</param>
    /// <param name="Address">Полный адрес объекта недвижимости</param>
    public record ResidenceCommandResultDto(
        long Id,
        string Address
    );
}
