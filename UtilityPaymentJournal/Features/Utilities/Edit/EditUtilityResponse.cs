namespace UtilityPaymentJournal.Features.Utilities.Edit
{
    /// <summary>
    /// Ответ API, содержащий отредактированные данные услуги.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор услуги.</param>
    /// <param name="Name">Новое наименование услуги.</param>
    /// <param name="IconClass">Новый класс иконки Bootstrap Icons</param>
    /// <param name="IsActive">Новый статус активности услуги</param>
    public record EditUtilityResponse(
        long Id, 
        string Name,
        string IconClass,
        bool IsActive
    );
}
