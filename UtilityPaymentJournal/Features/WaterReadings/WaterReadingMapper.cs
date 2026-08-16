using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Features.WaterReadings.Commands;
using UtilityPaymentJournal.Features.WaterReadings.Models;
using UtilityPaymentJournal.Features.WaterReadings.Queries;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;


namespace UtilityPaymentJournal.Features.WaterReadings
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
                SubmissionDate: createViewModel.SubmissionDate,
                PaymentDate: createViewModel.PaymentDate,
                CurrentValue: createViewModel.CurrentValue,
                PreviousValue: createViewModel.PreviousValue,
                ResultValue: createViewModel.ResultValue,
                PaymentAmount: createViewModel.PaymentAmount
            );
        }

        public EditWaterReadingDto ToDto(EditWaterReadingViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditWaterReadingDto(
                ResidenceId: editViewModel.ResidenceId,
                UtilityProviderId: editViewModel.UtilityProviderId,
                WaterType: editViewModel.WaterType,
                SubmissionDate: editViewModel.SubmissionDate,
                PaymentDate: editViewModel.PaymentDate,
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
                WaterType = createDto.WaterType,
                SubmissionDate = createDto.SubmissionDate.ToUtcKind(),
                PaymentDate = createDto.PaymentDate.ToUtcKind(),
                CurrentValue = createDto.CurrentValue,
                PreviousValue = createDto.PreviousValue,
                ResultValue = createDto.ResultValue,
                PaymentAmount = createDto.PaymentAmount
            };
        }

        public void UpdateEntity(EditWaterReadingDto editDto, WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.ResidenceId = editDto.ResidenceId;
            entity.UtilityProviderId = editDto.UtilityProviderId;
            entity.WaterType = editDto.WaterType;
            entity.SubmissionDate = editDto.SubmissionDate.ToUtcKind();
            entity.PaymentDate = editDto.PaymentDate.ToUtcKind();
            entity.CurrentValue = editDto.CurrentValue;
            entity.PreviousValue = editDto.PreviousValue;
            entity.ResultValue = editDto.ResultValue;
            entity.PaymentAmount = editDto.PaymentAmount;
        }

        public WaterReadingCommandResultDto ToCommandResultDto(WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WaterReadingCommandResultDto(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                WaterType: entity.WaterType,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        public WaterReadingQueryResultDto ToQueryResultDto(WaterReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WaterReadingQueryResultDto(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                WaterType: entity.WaterType,
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

        public WaterReadingCreatedViewModel ToCreatedViewModel(WaterReadingCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new WaterReadingCreatedViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                WaterType = dto.WaterType,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public WaterReadingUpdatedViewModel ToUpdatedViewModel(WaterReadingCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new WaterReadingUpdatedViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                WaterType = dto.WaterType,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public WaterReadingDetailsViewModel ToViewModel(WaterReadingQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new WaterReadingDetailsViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                WaterType = dto.WaterType,
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
    }
}
