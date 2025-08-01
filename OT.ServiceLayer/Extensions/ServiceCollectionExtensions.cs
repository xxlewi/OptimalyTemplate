using Microsoft.Extensions.DependencyInjection;
using OT.ServiceLayer.Interfaces;
using OT.ServiceLayer.Mapping;
using OT.ServiceLayer.Services;

namespace OT.ServiceLayer.Extensions;

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