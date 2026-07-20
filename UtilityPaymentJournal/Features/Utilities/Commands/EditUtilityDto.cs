namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    /// <summary>
    /// ДТО для редактирования существующей коммунальной услуги.
    /// </summary>
    /// <param name="Name">Новое наименование коммунальной услуги</param>
    /// <param name="IconClass">Новый класс иконки Bootstrap Icons</param>
    /// <param name="IsActive">Новый статус активности услуги</param>
    public record EditUtilityDto(
        string Name,
        string IconClass,
        bool IsActive
    );
}
