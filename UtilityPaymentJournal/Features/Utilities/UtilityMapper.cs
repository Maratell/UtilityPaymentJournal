using UtilityPaymentJournal.Features.Utilities.Commands;
using UtilityPaymentJournal.Features.Utilities.Models;
using UtilityPaymentJournal.Features.Utilities.Queries;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities
{
    public class UtilityMapper : IUtilityMapper
    {
        public CreateUtilityDto ToDto(CreateUtilityViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateUtilityDto(
                Name: createViewModel.Name,
                IconClass: createViewModel.IconClass,
                IsActive: createViewModel.IsActive
            );
        }

        public EditUtilityDto ToDto(EditUtilityViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditUtilityDto(
                Name: editViewModel.Name,
                IconClass: editViewModel.IconClass,
                IsActive: editViewModel.IsActive
            );
        }

        public Utility ToEntity(CreateUtilityDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new Utility
            {
                Name = createDto.Name,
                IconClass = createDto.IconClass,
                IsActive = createDto.IsActive
            };
        }

        public void UpdateEntity(EditUtilityDto editDto, Utility entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = editDto.Name;
            entity.IconClass = editDto.IconClass;
            entity.IsActive = editDto.IsActive;
        }

        public UtilityCommandResultDto ToCommandResultDto(Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new UtilityCommandResultDto(
                Id: entity.Id,
                Name: entity.Name,
                IconClass: entity.IconClass,
                IsActive: entity.IsActive
            );
        }

        public UtilityQueryResultDto ToQueryResultDto(Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new UtilityQueryResultDto(
                Id: entity.Id,
                Name: entity.Name,
                IconClass: entity.IconClass,
                IsActive: entity.IsActive
            );
        }

        public UtilityCreatedViewModel ToCreatedViewModel(UtilityCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityCreatedViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IconClass = dto.IconClass,
                IsActive = dto.IsActive
            };
        }

        public UtilityUpdatedViewModel ToUpdatedViewModel(UtilityCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityUpdatedViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IconClass = dto.IconClass,
                IsActive = dto.IsActive
            };
        }

        public UtilityDetailsViewModel ToViewModel(UtilityQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IconClass = dto.IconClass,
                IsActive = dto.IsActive
            };
        }
    }
}
