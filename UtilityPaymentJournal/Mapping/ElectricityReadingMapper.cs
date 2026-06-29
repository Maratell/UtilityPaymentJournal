using UtilityPaymentJournal.DTO.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Extensions;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Mapping
{
    public class ElectricityReadingMapper : IElectricityReadingMapper
    {
        public CreateElectricityReadingDTO ToDto(CreateElectricityReadingViewModel vm)
        {
            if (vm == null)
                return null!;

            return new CreateElectricityReadingDTO
            {
                ResidenceId = vm.ResidenceId ?? 0,
                UtilityProviderId = vm.UtilityProviderId ?? 0,

                PaymentDate = vm.PaymentDate,
                SubmissionDate = vm.SubmissionDate,

                CurrentValue = vm.CurrentValue,
                PreviousValue = vm.PreviousValue,
                ResultValue = vm.ResultValue,

                PaymentAmount = vm.PaymentAmount
            };
        }

        public ElectricityReadingDTO ToDto(ElectricityReading entity)
        {
            if (entity == null)
                return null!;

            return new ElectricityReadingDTO
            {
                Id = entity.Id,

                ResidenceId = entity.ResidenceId,
                UtilityProviderId = entity.UtilityProviderId,

                ResidenceAddress = entity.Residence?.Address,
                UtilityProviderName = entity.UtilityProvider?.Name,

                SubmissionDate = entity.SubmissionDate,
                PaymentDate = entity.PaymentDate,

                CurrentValue = entity.CurrentValue,
                PreviousValue = entity.PreviousValue,
                ResultValue = entity.ResultValue,

                PaymentAmount = entity.PaymentAmount
            };
        }

        public EditElectricityReadingDTO ToDto(EditElectricityReadingViewModel vm)
        {
            if (vm == null)
                return null!;

            return new EditElectricityReadingDTO
            {
                Id = vm.Id,

                ResidenceId = vm.ResidenceId,
                UtilityProviderId = vm.UtilityProviderId,

                SubmissionDate = vm.SubmissionDate,
                PaymentDate = vm.PaymentDate,

                CurrentValue = vm.CurrentValue,
                PreviousValue = vm.PreviousValue,
                ResultValue = vm.ResultValue,

                PaymentAmount = vm.PaymentAmount
            };
        }

        public ElectricityReading ToEntity(CreateElectricityReadingDTO dto)
        {
            if (dto == null)
                return null!;

            return new ElectricityReading
            {
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,

                SubmissionDate = dto.SubmissionDate.ToUtcKind(),
                PaymentDate = dto.PaymentDate.ToUtcKind(),

                CurrentValue = dto.CurrentValue,
                PreviousValue = dto.PreviousValue,
                ResultValue = dto.ResultValue,

                PaymentAmount = dto.PaymentAmount
            };
        }

        public ElectricityReadingViewModel ToViewModel(ElectricityReadingDTO dto)
        {
            if (dto == null)
                return null!;

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

        public void UpdateEntity(EditElectricityReadingDTO dto, ElectricityReading entity)
        {
            entity.UtilityProviderId = dto.UtilityProviderId;
            entity.ResidenceId = dto.ResidenceId;

            entity.SubmissionDate = dto.SubmissionDate.ToUtcKind();
            entity.PaymentDate = dto.PaymentDate.ToUtcKind();

            entity.CurrentValue = dto.CurrentValue;
            entity.PreviousValue = dto.PreviousValue;
            entity.ResultValue = dto.ResultValue;

            entity.PaymentAmount = dto.PaymentAmount;

            //entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
