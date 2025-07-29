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
        
        // Add your custom entity mappings here
        // Example: CreateMap<Customer, CustomerDto>().ReverseMap();
    }
}