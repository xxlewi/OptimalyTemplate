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
        // PresentationLayer AutoMapper konfigurace
        services.AddAutoMapper(typeof(DtoToViewModelMappingProfile));
        
        // Configure Identity s enterprise-grade security settings
        services.AddDefaultIdentity<User>(options =>
        {
            // Account settings
            options.SignIn.RequireConfirmedAccount = false; // For development - disable email confirmation
            
            // Password policy - relaxed for development (tighten in production)
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            
            // User settings  
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }
}