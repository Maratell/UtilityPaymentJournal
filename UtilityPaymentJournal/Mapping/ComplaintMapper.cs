using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Features.Complaints.Commands;
using UtilityPaymentJournal.Features.Complaints.Models;
using UtilityPaymentJournal.Features.Complaints.Queries;
using UtilityPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Mapping
{
    public class ComplaintMapper : IComplaintMapper
    {
        public CreateComplaintDto ToDto(CreateComplaintViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateComplaintDto(
                Title: createViewModel.Title,
                Description: createViewModel.Description,
                UtilityId: createViewModel.UtilityId,
                SubmissionDate: createViewModel.SubmissionDate,
                IssueResolutionDate: createViewModel.IssueResolutionDate,
                Status: createViewModel.Status
            );
        }
        public EditComplaintDto ToDto(EditComplaintViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditComplaintDto(
                Title: editViewModel.Title,
                Description: editViewModel.Description,
                UtilityId: editViewModel.UtilityId,
                SubmissionDate: editViewModel.SubmissionDate,
                IssueResolutionDate: editViewModel.IssueResolutionDate,
                Status: editViewModel.Status
            );
        }

        public ChangeComplaintStatusDto ToDto(ChangeComplaintStatusViewModel changeStatusViewModel)
        {
            ArgumentNullException.ThrowIfNull(changeStatusViewModel);

            return new ChangeComplaintStatusDto(
                Id: changeStatusViewModel.Id,
                NewStatus: changeStatusViewModel.NewStatus
            );
        }

        public Complaint ToEntity(CreateComplaintDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new Complaint
            {
                Title = createDto.Title,
                Description = createDto.Description,
                UtilityId = createDto.UtilityId,
                SubmissionDate = createDto.SubmissionDate,
                IssueResolutionDate = createDto.IssueResolutionDate,
                Status = createDto.Status,
                CreatedAt = DateTime.UtcNow // Задаем системную дату при создании новой сущности
            };
        }

        public void UpdateEntity(EditComplaintDto editDto, Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Title = editDto.Title;
            entity.Description = editDto.Description;
            entity.UtilityId = editDto.UtilityId;
            entity.SubmissionDate = editDto.SubmissionDate;
            entity.IssueResolutionDate = editDto.IssueResolutionDate;
            entity.Status = editDto.Status;
        }

        public ComplaintCommandResultDto ToCommandResultDto(Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ComplaintCommandResultDto(
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
        public ComplaintQueryResultDto ToQueryResultDto(Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ComplaintQueryResultDto(
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
        public ComplaintCreatedViewModel ToCreatedViewModel(ComplaintCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ComplaintCreatedViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                UtilityId = dto.UtilityId,
                CreatedAt = dto.CreatedAt,
                SubmissionDate = dto.SubmissionDate,
                IssueResolutionDate = dto.IssueResolutionDate,
                Status = dto.Status
            };
        }

        public ComplaintUpdatedViewModel ToUpdatedViewModel(ComplaintCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ComplaintUpdatedViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                UtilityId = dto.UtilityId,
                CreatedAt = dto.CreatedAt,
                SubmissionDate = dto.SubmissionDate,
                IssueResolutionDate = dto.IssueResolutionDate,
                Status = dto.Status
            };
        }

        public ComplaintViewModel ToViewModel(ComplaintQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ComplaintViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                UtilityId = dto.UtilityId,
                UtilityName = dto.UtilityName,
                UtilityIcon = dto.UtilityIcon,
                CreatedAt = dto.CreatedAt,
                SubmissionDate = dto.SubmissionDate,
                IssueResolutionDate = dto.IssueResolutionDate,
                Status = dto.Status
            };
        }

        public ComplaintDetailsViewModel ToDetailsViewModel(ComplaintQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ComplaintDetailsViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                UtilityId = dto.UtilityId,
                UtilityName = dto.UtilityName,
                UtilityIcon = dto.UtilityIcon,
                CreatedAt = dto.CreatedAt,
                SubmissionDate = dto.SubmissionDate,
                IssueResolutionDate = dto.IssueResolutionDate,
                Status = dto.Status
            };
        }
    }
}
