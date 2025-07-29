using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OT.DataLayer.Data;
using OT.DataLayer.Entities;
using OT.PresentationLayer.Mapping;

namespace OT.PresentationLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ViewModelMappingProfile));
        
        // Configure Identity s naší vlastní User entitou
        services.AddDefaultIdentity<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }
}