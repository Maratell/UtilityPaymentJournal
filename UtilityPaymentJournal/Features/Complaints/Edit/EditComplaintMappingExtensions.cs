using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.Complaints.Edit
{
    /// <summary>
    /// Методы расширения для локального маппинга фичи редактирования.
    /// </summary>
    public static class EditComplaintMappingExtensions
    {
        /// <summary>
        /// Переносит измененные данные из команды в существующую доменную сущность.
        /// </summary>
        /// <param name="command">Команда <see cref="EditComplaintCommand"/> на редактирование поставщика услуг.</param>
        /// <param name="entity">Доменная сущность <see cref="Complaint"/>.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда или доменная сущность равны null.</exception>
        public static void UpdateEntity(this EditComplaintCommand command, Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.UtilityId = command.UtilityId;
            entity.SubmissionDate = command.SubmissionDate;
            entity.IssueResolutionDate = command.IssueResolutionDate;
            entity.Status = command.Status;
        }

        /// <summary>
        /// Преобразует обновленную доменную сущность в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="Complaint"/>.</param>
        /// <returns>Объект ответа API</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static EditComplaintResponse ToResponse(this Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new EditComplaintResponse(
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
