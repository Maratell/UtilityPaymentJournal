using AutoMapper;
using UtilityPaymentJournal.DTO;
using UtilityPaymentJournal.Models.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UtilityPaymentJournal.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Базовый маппинг (если имена свойств совпадают)
            CreateMap<CreateUserViewModel, CreateUserDTO>();
        }
    }
}
