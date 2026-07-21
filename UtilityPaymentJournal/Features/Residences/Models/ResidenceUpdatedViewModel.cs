namespace UtilityPaymentJournal.Features.Residences.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции обновления объекта недвижимости (ответ на PUT).
    /// Изолирована от модели создания для возможности независимого расширения метаданными апдейта.
    /// </summary>
    public class ResidenceUpdatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор измененной записи объекта недвижимости в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Новый полный адрес объекта недвижимости
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
