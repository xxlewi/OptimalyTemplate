using AutoMapper;
using OT.ServiceLayer.DTOs;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Mapping;

public class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        // Template mappings - remove in production
        CreateMap<TemplateProductDto, TemplateProductViewModel>()
            .ReverseMap();
            
        CreateMap<TemplateCategoryDto, TemplateCategoryViewModel>()
            .ReverseMap();
    }
}