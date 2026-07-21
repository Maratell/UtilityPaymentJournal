using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Features.Residences.Commands;
using UtilityPaymentJournal.Features.Residences.Models;
using UtilityPaymentJournal.Features.Residences.Queries;

namespace UtilityPaymentJournal.Features.Residences
{
    public class ResidenceMapper : IResidenceMapper
    {
        public CreateResidenceDto ToDto(CreateResidenceViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateResidenceDto(
                Address: createViewModel.Address
            );
        }

        public EditResidenceDto ToDto(EditResidenceViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditResidenceDto(
                Address: editViewModel.Address
            );
        }

        public Residence ToEntity(CreateResidenceDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new Residence
            {
                Address = createDto.Address
            };
        }

        public void UpdateEntity(EditResidenceDto editDto, Residence entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Address = editDto.Address;
        }

        public ResidenceCommandResultDto ToCommandResultDto(Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ResidenceCommandResultDto(
                Id: entity.Id,
                Address: entity.Address
            );
        }

        public ResidenceQueryResultDto ToQueryResultDto(Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ResidenceQueryResultDto(
                Id: entity.Id,
                Address: entity.Address
            );
        }

        public ResidenceCreatedViewModel ToCreatedViewModel(ResidenceCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ResidenceCreatedViewModel
            {
                Id = dto.Id,
                Address = dto.Address
            };
        }

        public ResidenceUpdatedViewModel ToUpdatedViewModel(ResidenceCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ResidenceUpdatedViewModel
            {
                Id = dto.Id,
                Address = dto.Address
            };
        }

        public ResidenceDetailsViewModel ToViewModel(ResidenceQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ResidenceDetailsViewModel
            {
                Id = dto.Id,
                Address = dto.Address
            };
        }
    }
}
