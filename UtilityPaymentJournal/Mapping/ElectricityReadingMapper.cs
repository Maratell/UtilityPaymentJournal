using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Mapping
{
    public class ElectricityReadingMapper : IElectricityReadingMapper
    {
        public CreateElectricityReadingDto ToDto(CreateElectricityReadingViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateElectricityReadingDto(
                ResidenceId: createViewModel.ResidenceId,
                UtilityProviderId: createViewModel.UtilityProviderId,
                PaymentDate: createViewModel.PaymentDate,
                SubmissionDate: createViewModel.SubmissionDate,
                CurrentValue: createViewModel.CurrentValue,
                PreviousValue: createViewModel.PreviousValue,
                ResultValue: createViewModel.ResultValue,
                PaymentAmount: createViewModel.PaymentAmount
            );
        }

        public ElectricityReadingDto ToDto(ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ElectricityReadingDto(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                ResidenceAddress: entity.Residence?.Address,
                UtilityProviderName: entity.UtilityProvider?.Name,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        public EditElectricityReadingDto ToDto(EditElectricityReadingViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditElectricityReadingDto(
            
                Id: editViewModel.Id,
                ResidenceId: editViewModel.ResidenceId,
                UtilityProviderId: editViewModel.UtilityProviderId,
                SubmissionDate: editViewModel.SubmissionDate,
                PaymentDate: editViewModel.PaymentDate,
                CurrentValue: editViewModel.CurrentValue,
                PreviousValue: editViewModel.PreviousValue,
                ResultValue: editViewModel.ResultValue,
                PaymentAmount: editViewModel.PaymentAmount
            );
        }

        public ElectricityReading ToEntity(CreateElectricityReadingDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new ElectricityReading
            {
                ResidenceId = createDto.ResidenceId,
                UtilityProviderId = createDto.UtilityProviderId,
                SubmissionDate = createDto.SubmissionDate.ToUtcKind(),
                PaymentDate = createDto.PaymentDate.ToUtcKind(),
                CurrentValue = createDto.CurrentValue,
                PreviousValue = createDto.PreviousValue,
                ResultValue = createDto.ResultValue,
                PaymentAmount = createDto.PaymentAmount
            };
        }

        public ElectricityReadingViewModel ToViewModel(ElectricityReadingDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ElectricityReadingViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                ResidenceAddress = dto.ResidenceAddress,
                UtilityProviderName = dto.UtilityProviderName,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public void UpdateEntity(EditElectricityReadingDto editDto, ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.UtilityProviderId = editDto.UtilityProviderId;
            entity.ResidenceId = editDto.ResidenceId;
            entity.SubmissionDate = editDto.SubmissionDate.ToUtcKind();
            entity.PaymentDate = editDto.PaymentDate.ToUtcKind();
            entity.CurrentValue = editDto.CurrentValue;
            entity.PreviousValue = editDto.PreviousValue;
            entity.ResultValue = editDto.ResultValue;
            entity.PaymentAmount = editDto.PaymentAmount;

            //entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
