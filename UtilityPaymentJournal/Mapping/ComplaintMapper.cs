using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Extensions;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Mapping
{
    public class ComplaintMapper : IComplaintMapper
    {
        public CreateComplaintDTO ToDto(CreateComplaintViewModel vm)
        {
            if (vm == null)
                return null!;

            return new CreateComplaintDTO
            {
                Title = vm.Title,
                Description = vm.Description,
                Status = vm.Status,
                UtilityId = vm.UtilityId,

                SubmissionDate = vm.SubmissionDate,
                IssueResolutionDate = vm.IssueResolutionDate,
                CreatedAt = vm.CreatedAt
            };
        }

        public ComplaintDTO ToDto(Complaint entity)
        {
            if (entity == null)
                return null!;

            return new ComplaintDTO
            {
                Id = entity.Id,

                Title = entity.Title,
                Description = entity.Description,
                Status = entity.Status,

                UtilityId = entity.UtilityId,
                UtilityName = entity.Utility?.Name,
                UtilityIcon = entity.Utility?.IconClass,

                SubmissionDate = entity.SubmissionDate.ToLocalTime(),
                IssueResolutionDate = entity.IssueResolutionDate.ToLocalTime(),
                CreatedAt = entity.CreatedAt.ToLocalTime()
            };
        }

        public EditComplaintDTO ToDto(EditComplaintViewModel vm)
        {
            if (vm == null)
                return null!;

            return new EditComplaintDTO
            {
                Id = vm.Id,

                Title = vm.Title,
                Description = vm.Description,
                Status = vm.Status,
                UtilityId = vm.UtilityId,

                SubmissionDate = vm.SubmissionDate,
                IssueResolutionDate = vm.IssueResolutionDate,
                CreatedAt = vm.CreatedAt
            };
        }

        public Complaint ToEntity(CreateComplaintDTO dto)
        {
            if (dto == null)
                return null!;

            return new Complaint
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                UtilityId = dto.UtilityId,

                SubmissionDate = dto.SubmissionDate?.ToUniversalTime(),
                IssueResolutionDate = dto.IssueResolutionDate?.ToUniversalTime(),
                CreatedAt = dto.CreatedAt.ToUniversalTime()
            };
        }

        public ComplaintViewModel ToViewModel(ComplaintDTO dto)
        {
            if (dto == null)
                return null!;

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

        public void UpdateEntity(EditComplaintDTO dto, Complaint entity)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.UtilityId = dto.UtilityId;

            //entity.Utility = null!;

            entity.SubmissionDate = dto.SubmissionDate.ToUniversalTime();
            entity.IssueResolutionDate = dto.IssueResolutionDate.ToUniversalTime();
        }
    }
}
