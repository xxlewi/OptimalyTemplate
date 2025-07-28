using AutoMapper;
using OT.ServiceLayer.DTOs;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Mapping;

public class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        CreateMap<ProductDto, ProductViewModel>().ReverseMap();
    }
}