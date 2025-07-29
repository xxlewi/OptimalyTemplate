using Microsoft.Extensions.DependencyInjection;
using OT.ServiceLayer.Mapping;

namespace OT.ServiceLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        
        // Register your services here
        // Example: services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}