using Microsoft.Extensions.DependencyInjection;
using OT.ServiceLayer.Interfaces;
using OT.ServiceLayer.Mapping;
using OT.ServiceLayer.Services;

namespace OT.ServiceLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        // AutoMapper konfigurace
        services.AddAutoMapper(typeof(MappingProfile));
        
        // User service registrace
        services.AddScoped<IUserService, UserService>();
        
        // Register your custom services here
        // Example: services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}