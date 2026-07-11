using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.DTOs.WaterReadings;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;

namespace WaterReadingPaymentJournal.Mapping
{
    public class WaterReadingMapper : IWaterReadingMapper
    {
        public CreateWaterReadingDto ToDto(CreateWaterReadingViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateWaterReadingDto(
                ResidenceId: createViewModel.ResidenceId,
                UtilityProviderId: createViewModel.UtilityProviderId,
                WaterType: createViewModel.WaterType,
                PaymentDate: createViewModel.PaymentDate,
                SubmissionDate: createViewModel.SubmissionDate,
                CurrentValue: createViewModel.CurrentValue,
                PreviousValue: createViewModel.PreviousValue,
                ResultValue: createViewModel.ResultValue,
                PaymentAmount: createViewModel.PaymentAmount
            );
        }

        public WaterReadingDto ToDto(WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WaterReadingDto(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                ResidenceAddress: entity.Residence?.Address,
                UtilityProviderName: entity.UtilityProvider?.Name,
                WaterType: entity.WaterType,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        public EditWaterReadingDto ToDto(EditWaterReadingViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditWaterReadingDto(
                Id: editViewModel.Id,
                ResidenceId: editViewModel.ResidenceId,
                UtilityProviderId: editViewModel.UtilityProviderId,
                SubmissionDate: editViewModel.SubmissionDate,
                PaymentDate: editViewModel.PaymentDate,
                WaterType: editViewModel.WaterType,
                CurrentValue: editViewModel.CurrentValue,
                PreviousValue: editViewModel.PreviousValue,
                ResultValue: editViewModel.ResultValue,
                PaymentAmount: editViewModel.PaymentAmount
            );
        }

        public WaterReading ToEntity(CreateWaterReadingDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new WaterReading
            {
                ResidenceId = createDto.ResidenceId,
                UtilityProviderId = createDto.UtilityProviderId,
                SubmissionDate = createDto.SubmissionDate.ToUtcKind(),
                PaymentDate = createDto.PaymentDate.ToUtcKind(),
                WaterType = createDto.WaterType,
                CurrentValue = createDto.CurrentValue,
                PreviousValue = createDto.PreviousValue,
                ResultValue = createDto.ResultValue,
                PaymentAmount = createDto.PaymentAmount
            };
        }

        public WaterReadingViewModel ToViewModel(WaterReadingDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new WaterReadingViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                ResidenceAddress = dto.ResidenceAddress,
                UtilityProviderName = dto.UtilityProviderName,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                WaterType = dto.WaterType,
                CurrentValue = dto.CurrentValue,
                PreviousValue= dto.PreviousValue,
                ResultValue= dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public void UpdateEntity(EditWaterReadingDto editDto, WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.UtilityProviderId = editDto.UtilityProviderId;
            entity.ResidenceId = editDto.ResidenceId;
            entity.WaterType = editDto.WaterType;
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
