using Microsoft.Extensions.DependencyInjection;
using OT.ServiceLayer.Mapping;
using OT.ServiceLayer.Interfaces;
using OT.ServiceLayer.Services;

namespace OT.ServiceLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}