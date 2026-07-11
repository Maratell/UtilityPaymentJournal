using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.ComplaintBoard;

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
                Status: createViewModel.Status,
                UtilityId: createViewModel.UtilityId,
                SubmissionDate: createViewModel.SubmissionDate,
                IssueResolutionDate: createViewModel.IssueResolutionDate,
                CreatedAt: createViewModel.CreatedAt
            );
        }

        public ComplaintDto ToDto(Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ComplaintDto(
                Id: entity.Id,
                Title: entity.Title,
                Description: entity.Description,
                Status: entity.Status,
                UtilityId: entity.UtilityId,
                UtilityName: entity.Utility?.Name,
                UtilityIcon: entity.Utility?.IconClass,
                SubmissionDate: entity.SubmissionDate.ToLocalTime(),
                IssueResolutionDate: entity.IssueResolutionDate.ToLocalTime(),
                CreatedAt: entity.CreatedAt.ToLocalTime()
            );
        }

        public EditComplaintDto ToDto(EditComplaintViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditComplaintDto(
                Id: editViewModel.Id,
                Title: editViewModel.Title,
                Description: editViewModel.Description,
                Status: editViewModel.Status,
                UtilityId: editViewModel.UtilityId,
                SubmissionDate: editViewModel.SubmissionDate,
                IssueResolutionDate: editViewModel.IssueResolutionDate,
                CreatedAt: editViewModel.CreatedAt
            );
        }

        public EditComplaintDto ToDto(ComplaintDto dto, ComplaintStatus status)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new EditComplaintDto(
                Id: dto.Id,
                Title: dto.Title,
                Description: dto.Description,
                UtilityId: dto.UtilityId,
                Status: status, // Устанавливаем новый статус
                CreatedAt: dto.CreatedAt,
                SubmissionDate: dto.SubmissionDate,
                IssueResolutionDate: dto.IssueResolutionDate
            );
        }

        public Complaint ToEntity(CreateComplaintDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new Complaint
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Status = createDto.Status,
                UtilityId = createDto.UtilityId,
                SubmissionDate = createDto.SubmissionDate.ToUtcKind(),
                IssueResolutionDate = createDto.IssueResolutionDate.ToUtcKind(),
                CreatedAt = createDto.CreatedAt.ToUtcKind()
            };
        }

        public ComplaintViewModel ToViewModel(ComplaintDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ComplaintViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                UtilityName = dto.UtilityName,
                UtilityIcon = dto.UtilityIcon,
                UtilityId = dto.UtilityId,
                SubmissionDate = dto.SubmissionDate,
                IssueResolutionDate = dto.IssueResolutionDate,
                CreatedAt = dto.CreatedAt
            };
        }

        public void UpdateEntity(EditComplaintDto editDto, Complaint entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Title = editDto.Title;
            entity.Description = editDto.Description;
            entity.Status = editDto.Status;
            entity.UtilityId = editDto.UtilityId;
            entity.SubmissionDate = editDto.SubmissionDate.ToUtcKind();
            entity.IssueResolutionDate = editDto.IssueResolutionDate.ToUtcKind();
        }
    }
}
