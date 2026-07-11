using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IElectricityReadingMapper
    {
        CreateElectricityReadingDto ToDto(CreateElectricityReadingViewModel createViewModel);
        ElectricityReadingDto ToDto(ElectricityReading entity);
        EditElectricityReadingDto ToDto(EditElectricityReadingViewModel editViewModel);
        ElectricityReading ToEntity(CreateElectricityReadingDto createDto);
        ElectricityReadingViewModel ToViewModel(ElectricityReadingDto dto);
        void UpdateEntity(EditElectricityReadingDto editDto, ElectricityReading entity);
    }
}
