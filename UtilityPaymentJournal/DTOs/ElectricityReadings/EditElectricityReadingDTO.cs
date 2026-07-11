
namespace UtilityPaymentJournal.DTOs.ElectricityReadings
{
    /// <summary>
    /// ДТО для редактирования существующего показания счетчика электроэнергии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор записи показания в бд</param>
    /// <param name="ResidenceId">Уникальный идентификатор объекта недвижимости (опционально)</param>
    /// <param name="UtilityProviderId">Уникальный идентификатор поставщика услуг (опционально)</param>
    /// <param name="SubmissionDate">Новая дата подачи показаний (может быть null, если еще не подано)</param>
    /// <param name="PaymentDate">Новая дата оплаты (может быть null, если еще не оплачено)</param>
    /// <param name="CurrentValue">Новое текущее показание счетчика</param>
    /// <param name="PreviousValue">Новое предыдущее показание счетчика</param>
    /// <param name="ResultValue">Новая разница показаний (расход за период)</param>
    /// <param name="PaymentAmount">Новая сумма платежа</param>
    public record EditElectricityReadingDto(
        long Id,
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
