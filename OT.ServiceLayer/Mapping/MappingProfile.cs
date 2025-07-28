using AutoMapper;
using OT.DataLayer.Entities;
using OT.ServiceLayer.DTOs;

namespace OT.ServiceLayer.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}