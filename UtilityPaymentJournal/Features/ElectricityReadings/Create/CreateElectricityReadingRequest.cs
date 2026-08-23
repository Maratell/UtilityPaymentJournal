
namespace UtilityPaymentJournal.Features.ElectricityReadings.Create
{
    /// <summary>
    /// Запрос на создание показания счетчика электроэнергии.
    /// </summary>
    /// <param name="ResidenceId">Идентификатор жилого объекта</param>
    /// <param name="UtilityProviderId">Идентификатор поставщика услуг</param>
    /// <param name="SubmissionDate">Дата подачи показаний (null, если еще не подано)</param>
    /// <param name="PaymentDate">Дата оплаты (null, если еще не оплачено)</param>
    /// <param name="CurrentValue">Текущее показание счетчика</param>
    /// <param name="PreviousValue">Предыдущее показание счетчика</param>
    /// <param name="ResultValue">Разница показаний (расход за текущий период)</param>
    /// <param name="PaymentAmount">Сумма платежа за расчетный объем</param>
    public record CreateElectricityReadingRequest(
        long? ResidenceId,
        long? UtilityProviderId,
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    )
    {
        public CreateElectricityReadingCommand ToCommand() =>
            new CreateElectricityReadingCommand(
                ResidenceId,
                UtilityProviderId,
                SubmissionDate,
                PaymentDate,
                CurrentValue,
                PreviousValue,
                ResultValue,
                PaymentAmount
            );
    }
}
