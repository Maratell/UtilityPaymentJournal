namespace UtilityPaymentJournal.Features.Residences.Models
{
    /// <summary>
    /// Развернутая модель представления объекта недвижимости для отображения на UI (ответ на GET).
    /// Содержит полную текстовую и идентификационную информацию о жилом объекте.
    /// </summary>
    public class ResidenceDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи объекта недвижимости в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Полный текстовый адрес объекта недвижимости
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
