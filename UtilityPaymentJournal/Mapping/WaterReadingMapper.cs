using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;

namespace WaterReadingPaymentJournal.Mapping
{
    public class WaterReadingMapper : IWaterReadingMapper
    {
        public CreateWaterReadingDTO ToDto(CreateWaterReadingViewModel vm)
        {
            if (vm == null)
                return null!;

            return new CreateWaterReadingDTO
            {
                ResidenceId = vm.ResidenceId ?? 0,
                UtilityProviderId = vm.UtilityProviderId ?? 0,

                WaterType = vm.WaterType,
                
                PaymentDate = vm.PaymentDate,
                SubmissionDate = vm.SubmissionDate,

                CurrentValue = vm.CurrentValue,
                PreviousValue = vm.PreviousValue,
                ResultValue = vm.ResultValue,

                PaymentAmount = vm.PaymentAmount
            };
        }

        public WaterReadingDTO ToDto(WaterReading entity)
        {
            if (entity == null)
                return null!;

            return new WaterReadingDTO
            {
                Id = entity.Id,

                ResidenceId = entity.ResidenceId,
                UtilityProviderId = entity.UtilityProviderId,

                ResidenceAddress = entity.Residence?.Address,
                UtilityProviderName = entity.UtilityProvider?.Name,

                SubmissionDate = entity.SubmissionDate,
                PaymentDate = entity.PaymentDate,

                WaterType = entity.WaterType,

                CurrentValue = entity.CurrentValue,
                PreviousValue = entity.PreviousValue,
                ResultValue = entity.ResultValue,

                PaymentAmount = entity.PaymentAmount
            };
        }

        public EditWaterReadingDTO ToDto(EditWaterReadingViewModel vm)
        {
            if (vm == null)
                return null!;

            return new EditWaterReadingDTO
            {
                Id = vm.Id,

                ResidenceId = vm.ResidenceId,
                UtilityProviderId = vm.UtilityProviderId,

                SubmissionDate = vm.SubmissionDate,
                PaymentDate = vm.PaymentDate,

                WaterType = vm.WaterType,

                CurrentValue = vm.CurrentValue,
                PreviousValue = vm.PreviousValue,
                ResultValue = vm.ResultValue,

                PaymentAmount =  vm.PaymentAmount
            };
        }

        public WaterReading ToEntity(CreateWaterReadingDTO dto)
        {
            if (dto == null)
                return null!;

            return new WaterReading
            {
                ResidenceId = dto.ResidenceId,
                UtilityProviderId = dto.UtilityProviderId,

                SubmissionDate = dto.SubmissionDate.ToUtcKind(),
                PaymentDate = dto.PaymentDate.ToUtcKind(),

                WaterType = dto.WaterType,

                CurrentValue = dto.CurrentValue,
                PreviousValue= dto.PreviousValue,
                ResultValue= dto.ResultValue,

                PaymentAmount = dto.PaymentAmount
            };
        }

        public WaterReadingViewModel ToViewModel(WaterReadingDTO dto)
        {
            if (dto == null)
                return null!;

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

        public void UpdateEntity(EditWaterReadingDTO dto, WaterReading entity)
        {
            entity.UtilityProviderId = dto.UtilityProviderId;
            entity.ResidenceId = dto.ResidenceId;

            entity.WaterType = dto.WaterType;

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
