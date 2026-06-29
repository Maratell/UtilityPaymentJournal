using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;

namespace WaterReadingPaymentJournal.Interface.Mapping
{
    public interface IWaterReadingMapper
    {
        CreateWaterReadingDTO ToDto(CreateWaterReadingViewModel vm);

        WaterReadingDTO ToDto(WaterReading entity);

        EditWaterReadingDTO ToDto(EditWaterReadingViewModel vm);

        WaterReading ToEntity(CreateWaterReadingDTO dto);

        WaterReadingViewModel ToViewModel(WaterReadingDTO dto);

        void UpdateEntity(EditWaterReadingDTO dto, WaterReading entity);
    }
}
