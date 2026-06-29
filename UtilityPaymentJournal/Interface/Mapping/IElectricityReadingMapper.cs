

using UtilityPaymentJournal.DTO.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IElectricityReadingMapper
    {
        CreateElectricityReadingDTO ToDto(CreateElectricityReadingViewModel vm);

        ElectricityReadingDTO ToDto(ElectricityReading entity);

        EditElectricityReadingDTO ToDto(EditElectricityReadingViewModel vm);

        ElectricityReading ToEntity(CreateElectricityReadingDTO dto);

        ElectricityReadingViewModel ToViewModel(ElectricityReadingDTO dto);

        void UpdateEntity(EditElectricityReadingDTO dto, ElectricityReading entity);
    }
}
