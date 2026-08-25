using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.Complaints.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей карточки жалобы.
    /// </summary>
    public static class GetComplaintByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность карточки жалобы в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="Complaint"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetComplaintByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равны null.</exception>
        public static GetComplaintByIdResponse ToResponse(this Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetComplaintByIdResponse(
                Id: entity.Id,
                Title: entity.Title,
                Description: entity.Description,
                UtilityId: entity.UtilityId,
                UtilityName: entity.Utility?.Name,       // Мапим из навигационного свойства
                UtilityIcon: entity.Utility?.IconClass,  // Мапим из навигационного свойства
                CreatedAt: entity.CreatedAt,
                SubmissionDate: entity.SubmissionDate,
                IssueResolutionDate: entity.IssueResolutionDate,
                Status: entity.Status
            );
        }
    }
}
