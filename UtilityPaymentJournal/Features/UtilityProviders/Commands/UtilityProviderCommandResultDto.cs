namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    /// <summary>
    /// ДТО для возврата данных о поставщике коммунальных услуг после выполнения команды записи (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор поставщика</param>
    /// <param name="Name">Наименование поставщика услуг</param>
    public record UtilityProviderCommandResultDto(
        long Id,
        string Name
    );
}