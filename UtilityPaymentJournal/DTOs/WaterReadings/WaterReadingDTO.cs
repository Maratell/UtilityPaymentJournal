using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.WaterReadings
{
    /// <summary>
    /// ДТО для возврата данных о показаниях счетчика воды (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор записи показания</param>
    /// <param name="ResidenceId">Идентификатор связанного жилого объекта</param>
    /// <param name="UtilityProviderId">Идентификатор связанного поставщика услуг</param>
    /// <param name="ResidenceAddress">Полный адрес жилого объекта (для отображения на клиенте)</param>
    /// <param name="UtilityProviderName">Наименование поставщика услуг (для отображения на клиенте)</param>
    /// <param name="WaterType">Тип воды (холодная/горячая/канализация)</param>
    /// <param name="SubmissionDate">Дата подачи показаний (null, если не подано)</param>
    /// <param name="PaymentDate">Дата оплаты (null, если не оплачено)</param>
    /// <param name="CurrentValue">Текущее показание счетчика</param>
    /// <param name="PreviousValue">Предыдущее показание счетчика</param>
    /// <param name="ResultValue">Разница показаний (расход за период)</param>
    /// <param name="PaymentAmount">Сумма платежа</param>
    public record WaterReadingDto(
        long Id,
        long? ResidenceId,
        long? UtilityProviderId,
        string? ResidenceAddress,
        string? UtilityProviderName,
        WaterType WaterType,
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    );
}
