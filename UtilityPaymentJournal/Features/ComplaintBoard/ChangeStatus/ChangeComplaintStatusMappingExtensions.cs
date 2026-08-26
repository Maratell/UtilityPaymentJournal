
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи изменения статуса карточки жалобы.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class ChangeComplaintStatusMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность карточки жалобы в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="Complaint"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="ChangeComplaintStatusResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static ChangeComplaintStatusResponse ToResponse(this Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ChangeComplaintStatusResponse(
                Id: entity.Id,
                Title: entity.Title,
                Description: entity.Description,
                UtilityId: entity.UtilityId,
                CreatedAt: entity.CreatedAt,
                SubmissionDate: entity.SubmissionDate,
                IssueResolutionDate: entity.IssueResolutionDate,
                Status: entity.Status
            );
        }
    }
}
