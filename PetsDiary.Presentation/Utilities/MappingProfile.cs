using AutoMapper;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.ViewModels;

namespace PetsDiary.Presentation.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VisitModel, VisitViewModel>().ReverseMap();
            CreateMap<VaccinationModel, VaccinationViewModel>().ReverseMap();
            CreateMap<WeightModel, WeightViewModel>().ReverseMap();
            CreateMap<AnimalModel, AnimalViewModel>().ReverseMap();
            CreateMap<NoteModel, NoteViewModel>().ReverseMap();
        }
    }
}