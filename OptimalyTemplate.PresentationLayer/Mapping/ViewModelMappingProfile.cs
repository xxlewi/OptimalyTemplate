using AutoMapper;
using OptimalyTemplate.ServiceLayer.DTOs;
using OptimalyTemplate.PresentationLayer.ViewModels;

namespace OptimalyTemplate.PresentationLayer.Mapping;

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