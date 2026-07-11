using AutoMapper;
using UtilityPaymentJournal.DTOs.Admin;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Базовый маппинг (если имена свойств совпадают)
            CreateMap<CreateUserViewModel, CreateUserDto>();
        }
    }
}
