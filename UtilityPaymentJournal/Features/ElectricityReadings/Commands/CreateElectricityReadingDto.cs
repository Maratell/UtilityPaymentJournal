namespace UtilityPaymentJournal.Features.ElectricityReadings.Commands
{
    /// <summary>
    /// ДТО для создания нового показания счетчика электроэнергии.
    /// </summary>
    /// <param name="ResidenceId">Уникальный идентификатор объекта недвижимости</param>
    /// <param name="UtilityProviderId">Уникальный идентификатор поставщика услуг</param>
    /// <param name="SubmissionDate">Дата подачи показаний (может быть null, если еще не подано)</param>
    /// <param name="PaymentDate">Дата оплаты (может быть null, если еще не оплачено)</param>
    /// <param name="CurrentValue">Текущее показание счетчика</param>
    /// <param name="PreviousValue">Предыдущее показание счетчика</param>
    /// <param name="ResultValue">Разница показаний (расход за период)</param>
    /// <param name="PaymentAmount">Сумма платежа</param>
    public record CreateElectricityReadingDto(
        long? ResidenceId,
        long? UtilityProviderId,
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    );
}
