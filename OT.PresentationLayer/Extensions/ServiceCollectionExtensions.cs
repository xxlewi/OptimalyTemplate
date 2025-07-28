using Microsoft.Extensions.DependencyInjection;
using OT.PresentationLayer.Mapping;

namespace OT.PresentationLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ViewModelMappingProfile));
        
        return services;
    }
}