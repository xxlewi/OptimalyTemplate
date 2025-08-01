using AutoMapper;
using OT.ServiceLayer.DTOs;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Mapping;

/// <summary>
/// Presentation layer mapping profile - DTO to ViewModel mappings  
/// Maps between data transfer objects and view models for UI
/// </summary>
public class DtoToViewModelMappingProfile : Profile
{
    public DtoToViewModelMappingProfile()
    {
        // Template mappings - remove in production
        CreateMap<TemplateProductDto, TemplateProductViewModel>()
            .ReverseMap();
            
        CreateMap<TemplateCategoryDto, TemplateCategoryViewModel>()
            .ReverseMap();
    }
}