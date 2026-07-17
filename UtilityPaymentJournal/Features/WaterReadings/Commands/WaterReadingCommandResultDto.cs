using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.WaterReadings.Commands
{
    /// <summary>
    /// ДТО для возврата данных о показаниях счетчика воды (ответ API на команды записи).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор записи показания в БД</param>
    /// <param name="ResidenceId">Идентификатор жилого объекта</param>
    /// <param name="UtilityProviderId">Идентификатор поставщика услуг</param>
    /// <param name="WaterType">Тип воды (холодная/горячая)</param>
    /// <param name="SubmissionDate">Дата подачи показаний (null, если еще не подано)</param>
    /// <param name="PaymentDate">Дата оплаты (null, если еще не оплачено)</param>
    /// <param name="CurrentValue">Текущее показание счетчика</param>
    /// <param name="PreviousValue">Предыдущее показание счетчика</param>
    /// <param name="ResultValue">Разница показаний (расход за текущий период)</param>
    /// <param name="PaymentAmount">Сумма платежа за расчетный объем</param>
    public record WaterReadingCommandResultDto(
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
