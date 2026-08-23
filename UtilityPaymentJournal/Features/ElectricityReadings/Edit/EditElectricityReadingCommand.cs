using MediatR;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Edit
{
    /// <summary>
    /// Команда на редактирование данных показания счетчика электроэнергии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор показания счетчика электроэнергии.</param>
    /// <param name="ResidenceId">Новый идентификатор жилого объекта (при изменении привязки)</param>
    /// <param name="UtilityProviderId">Новый идентификатор поставщика услуг (при изменении привязки)</param>
    /// <param name="SubmissionDate">Новая дата подачи показаний</param>
    /// <param name="PaymentDate">Новая дата оплаты</param>
    /// <param name="CurrentValue">Новое текущее показание счетчика</param>
    /// <param name="PreviousValue">Новое предыдущее показание счетчика</param>
    /// <param name="ResultValue">Новая разница показаний (расход)</param>
    /// <param name="PaymentAmount">Новая сумма платежа</param>
    public record EditElectricityReadingCommand(
        long Id,
        long? ResidenceId,
        long? UtilityProviderId,
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    ) : IRequest<EditElectricityReadingResponse>;
}
