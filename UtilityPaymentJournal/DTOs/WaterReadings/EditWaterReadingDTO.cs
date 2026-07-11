using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.WaterReadings
{
    /// <summary>
    /// ДТО для редактирования существующего показания счетчика воды.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор записи показания в БД</param>
    /// <param name="ResidenceId">Новый идентификатор жилого объекта (при изменении привязки)</param>
    /// <param name="UtilityProviderId">Новый идентификатор поставщика услуг (при изменении привязки)</param>
    /// <param name="WaterType">Тип воды (холодная/горячая)</param>
    /// <param name="SubmissionDate">Новая дата подачи показаний</param>
    /// <param name="PaymentDate">Новая дата оплаты</param>
    /// <param name="CurrentValue">Новое текущее показание счетчика</param>
    /// <param name="PreviousValue">Новое предыдущее показание счетчика</param>
    /// <param name="ResultValue">Новая разница показаний (расход)</param>
    /// <param name="PaymentAmount">Новая сумма платежа</param>
    public record EditWaterReadingDto(
        long Id,
        long? ResidenceId,
        long? UtilityProviderId,
        WaterType WaterType,
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    );
}
