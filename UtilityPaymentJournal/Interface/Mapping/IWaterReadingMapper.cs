using UtilityPaymentJournal.DTOs.WaterReadings;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;

namespace WaterReadingPaymentJournal.Interface.Mapping
{
    public interface IWaterReadingMapper
    {
        CreateWaterReadingDto ToDto(CreateWaterReadingViewModel createViewModel);
        WaterReadingDto ToDto(WaterReading entity);
        EditWaterReadingDto ToDto(EditWaterReadingViewModel editViewModel);
        WaterReading ToEntity(CreateWaterReadingDto createDto);
        WaterReadingViewModel ToViewModel(WaterReadingDto dto);
        void UpdateEntity(EditWaterReadingDto editDto, WaterReading entity);
    }
}
