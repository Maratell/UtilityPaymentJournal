using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания карточки жалобы.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateComplaintMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность карточки жалобы в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="Complaint"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateComplaintResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateComplaintResponse ToResponse(this Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateComplaintResponse(
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

        /// <summary>
        /// Создает новую доменную сущность карточки жалобы на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания объекта.</param>
        /// <returns>Новый экземпляр <see cref="Complaint"/> готовый к сохранению в БД.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static Complaint ToEntity(this CreateComplaintCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new Complaint
            {
                Title = createCommand.Title,
                Description = createCommand.Description,
                UtilityId = createCommand.UtilityId!.Value, // ненулевое значение createCommand.UtilityId гарантирует валидация
                SubmissionDate = createCommand.SubmissionDate,
                IssueResolutionDate = createCommand.IssueResolutionDate,
                Status = createCommand.Status
            };
        }
    }
}
