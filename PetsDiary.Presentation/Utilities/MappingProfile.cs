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
                .ForMember(dest => dest.HasErrors, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.IsInEdit, opt => opt.Ignore())
                .DisableCtorValidation()
                .ReverseMap();
            CreateMap<VaccinationModel, VaccinationViewModel>()
                .ForMember(dest => dest.HasErrors, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.IsInEdit, opt => opt.Ignore())
                .DisableCtorValidation()
                .ReverseMap();
            CreateMap<WeightModel, WeightViewModel>()
                .ForMember(dest => dest.HasErrors, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.IsInEdit, opt => opt.Ignore())
                .ForMember(dest => dest.WeightText, opt => opt.Ignore())
                .DisableCtorValidation()
                .ReverseMap();
            CreateMap<PetModel, PetViewModel>()
                .ForMember(dest => dest.HasErrors, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.IsInEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanSave, opt => opt.Ignore())
                .ForMember(dest => dest.CancelCommand, opt => opt.Ignore())
                .ForMember(dest => dest.EditCommand, opt => opt.Ignore())
                .ForMember(dest => dest.CancelCommand, opt => opt.Ignore())
                .ForMember(dest => dest.ChangeImageCommand, opt => opt.Ignore())
                .ForMember(dest => dest.PetId, opt => opt.Ignore())
                .ForMember(dest => dest.SaveCommand, opt => opt.Ignore())
                .DisableCtorValidation()
                .ReverseMap();
            CreateMap<NoteModel, NoteViewModel>()
                .ForMember(dest => dest.HasErrors, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.IsInEdit, opt => opt.Ignore())
                .DisableCtorValidation()
                .ReverseMap();
        }
    }
}