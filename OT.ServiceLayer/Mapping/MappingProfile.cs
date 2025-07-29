using AutoMapper;
using OT.DataLayer.Entities;
using OT.ServiceLayer.DTOs;

namespace OT.ServiceLayer.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User entity mapping
        CreateMap<User, UserDto>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID se nenastavuje při vytváření
        
        // Template entity mappings - remove in production
        CreateMap<TemplateCategory, TemplateCategoryDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
            .ReverseMap()
            .ForMember(dest => dest.Products, opt => opt.Ignore()); // Don't map navigation property
            
        CreateMap<TemplateProduct, TemplateProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ReverseMap()
            .ForMember(dest => dest.Category, opt => opt.Ignore()); // Don't map navigation property
    }
}