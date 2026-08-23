
namespace UtilityPaymentJournal.Features.ElectricityReadings.GetById
{
    /// <summary>
    /// Ответ API, содержащий детальную информацию об одном показании счетчика электроэнергии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор показания счетчика электроэнергии, подтянутый из базы данных.</param>
    /// <param name="ResidenceId">Идентификатор связанного жилого помещения (дома/квартиры)</param>
    /// <param name="UtilityProviderId">Идентификатор связанного поставщика коммунальной услуги</param>
    /// <param name="ResidenceAddress">Полный текстовый адрес жилого помещения, подтянутый из БД</param>
    /// <param name="UtilityProviderName">Наименование компании поставщика услуги, подтянутое из БД</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи/фиксации показаний счетчика</param>
    /// <param name="PaymentDate">Дата и время проведения оплаты по данному показанию (опционально)</param>
    /// <param name="CurrentValue">Текущее зафиксированное значение на счетчике электроэнергии</param>
    /// <param name="PreviousValue">Предыдущее зафиксированное значение на счетчике для расчета разницы</param>
    /// <param name="ResultValue">Итоговый расход электроэнергии за расчетный период (CurrentValue - PreviousValue)</param>
    /// <param name="PaymentAmount">Сумма к оплате, рассчитанная на основе итогового расхода и тарифа</param>
    public record GetElectricityReadingByIdResponse(
        long Id,
        long? ResidenceId,
        long? UtilityProviderId,
        string? ResidenceAddress,     // Подтягивается из сущности Residence
        string? UtilityProviderName,  // Подтягивается из сущности UtilityProvider
        DateTime? SubmissionDate,
        DateTime? PaymentDate,
        long CurrentValue,
        long PreviousValue,
        long ResultValue,
        decimal PaymentAmount
    );
}
