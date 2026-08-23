
namespace UtilityPaymentJournal.Features.ElectricityReadings.Create
{
    /// <summary>
    /// ДТО ответа API, возвращающий данные созданного объекта показаня счетчика электроэнергии с присвоенным идентификатором.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор показаня счетчика электроэнергии, сгенерированный базой данных.</param>
    /// <param name="ResidenceId">Идентификатор жилого объекта</param>
    /// <param name="UtilityProviderId">Идентификатор поставщика услуг</param>
    /// <param name="SubmissionDate">Дата подачи показаний (null, если еще не подано)</param>
    /// <param name="PaymentDate">Дата оплаты (null, если еще не оплачено)</param>
    /// <param name="CurrentValue">Текущее показание счетчика</param>
    /// <param name="PreviousValue">Предыдущее показание счетчика</param>
    /// <param name="ResultValue">Разница показаний (расход за текущий период)</param>
    /// <param name="PaymentAmount">Сумма платежа за расчетный объем</param>
    public record CreateElectricityReadingResponse(
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
