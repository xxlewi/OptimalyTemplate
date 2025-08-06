using Microsoft.Extensions.DependencyInjection;
using OptimalyTemplate.ServiceLayer.Interfaces;
using OptimalyTemplate.ServiceLayer.Mapping;
using OptimalyTemplate.ServiceLayer.Services;

namespace OptimalyTemplate.ServiceLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        // ServiceLayer AutoMapper konfigurace
        services.AddAutoMapper(typeof(EntityToDtoMappingProfile));
        
        // User service registrace
        services.AddScoped<IUserService, UserService>();
        
        // Template services - remove in production
        services.AddScoped<ITemplateCategoryService, TemplateCategoryService>();
        services.AddScoped<ITemplateProductService, TemplateProductService>();
        
        // Enhanced services following CRM pattern
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IExportService, ExportService>();

        return services;
    }
}