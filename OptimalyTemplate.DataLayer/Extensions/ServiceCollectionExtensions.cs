using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OptimalyTemplate.DataLayer.Data;
using OptimalyTemplate.DataLayer.Interfaces;
using OptimalyTemplate.DataLayer.Repositories;

namespace OptimalyTemplate.DataLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Registrace UnitOfWork - poskytuje přístup ke všem repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Pozor: Repository už neregistrujeme separátně - jsou dostupné přes UnitOfWork
        // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        // services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}