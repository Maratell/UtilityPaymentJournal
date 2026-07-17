using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Features.ElectricityReadings.Commands;
using UtilityPaymentJournal.Features.ElectricityReadings.Models;
using UtilityPaymentJournal.Features.ElectricityReadings.Queries;
using UtilityPaymentJournal.Interface.Mapping;

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

        public EditElectricityReadingDto ToDto(EditElectricityReadingViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditElectricityReadingDto(
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
        }

        public ElectricityReadingCommandResultDto ToCommandResultDto(ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ElectricityReadingCommandResultDto(
                Id: entity.Id,
                ResidenceId: entity.ResidenceId,
                UtilityProviderId: entity.UtilityProviderId,
                SubmissionDate: entity.SubmissionDate,
                PaymentDate: entity.PaymentDate,
                CurrentValue: entity.CurrentValue,
                PreviousValue: entity.PreviousValue,
                ResultValue: entity.ResultValue,
                PaymentAmount: entity.PaymentAmount
            );
        }

        public ElectricityReadingQueryResultDto ToQueryResultDto(ElectricityReading entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ElectricityReadingQueryResultDto(
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

        public ElectricityReadingCreatedViewModel ToCreatedViewModel(ElectricityReadingCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ElectricityReadingCreatedViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public ElectricityReadingUpdatedViewModel ToUpdatedViewModel(ElectricityReadingCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ElectricityReadingUpdatedViewModel
            {
                Id = dto.Id,
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,
                SubmissionDate = dto.SubmissionDate,
                PaymentDate = dto.PaymentDate,
                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,
                PaymentAmount = dto.PaymentAmount
            };
        }

        public ElectricityReadingDetailsViewModel ToViewModel(ElectricityReadingQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new ElectricityReadingDetailsViewModel
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
    }
}
