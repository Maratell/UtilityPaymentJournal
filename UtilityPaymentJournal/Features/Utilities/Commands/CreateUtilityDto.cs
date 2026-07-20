namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    /// <summary>
    /// ДТО для создания новой коммунальной услуги.
    /// </summary>
    /// <param name="Name">Наименование коммунальной услуги</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons для отображения</param>
    /// <param name="IsActive">Статус активности услуги</param>
    public record CreateUtilityDto(
        string Name,
        string IconClass,
        bool IsActive = true
    );
}
