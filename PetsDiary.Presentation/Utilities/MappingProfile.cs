using AutoMapper;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.ViewModels;

namespace PetsDiary.Presentation.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VisitModel, VisitViewModel>()
                .ForMember(x=>x.Id, opt=>opt.MapFrom(y=>y.Id))
                .ReverseMap();
            CreateMap<VaccinationModel, VaccinationViewModel>().ReverseMap();
            CreateMap<WeightModel, WeightViewModel>().ReverseMap();
            CreateMap<AnimalModel, AnimalViewModel>().ReverseMap();
        }
    }
}