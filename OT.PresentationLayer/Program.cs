using OT.DataLayer.Data;
using OT.DataLayer.Extensions;
using OT.ServiceLayer.Extensions;
using OT.PresentationLayer.Extensions;
using OT.PresentationLayer.HealthChecks;
using OT.PresentationLayer.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

try
{
    Log.Information("Spouštění aplikace {ApplicationName}", "OptimalyTemplate");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // Add layers
    builder.Services.AddDataLayer(builder.Configuration);
    builder.Services.AddServiceLayer();
    builder.Services.AddPresentationLayer();

    // Add health checks
    builder.Services.AddHealthChecks()
        .AddCheck<ApplicationHealthCheck>("application")
        .AddDbContextCheck<ApplicationDbContext>("database")
        .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, "postgresql");

    var app = builder.Build();

    // Apply migrations automatically in development
    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                // Check if database can be connected and apply migrations if needed
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    app.Logger.LogInformation("Database migrations applied successfully");
                }
                else
                {
                    app.Logger.LogInformation("Database is up to date, no migrations needed");
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning(ex, "Error applying database migrations - continuing startup");
            }
        }
    }

    // Security headers middleware - musí být první
    app.UseMiddleware<SecurityHeadersMiddleware>();
    
    // Globální exception handling middleware
    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "Unknown");
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
            diagnosticContext.Set("UserAgent", userAgent);
        };
    });

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // Výchozí error handler nahradíme naším middleware
        // app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages();

    // Health check endpoints
    app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration.TotalMilliseconds,
                    exception = entry.Value.Exception?.Message
                }),
                totalDuration = report.TotalDuration.TotalMilliseconds
            };
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions 
            { 
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase 
            }));
        }
    });

    // Simple health check endpoint for load balancers
    app.MapHealthChecks("/health/ready");

    Log.Information("Aplikace úspěšně spuštěna");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikace selhala při startu");
}
finally
{
    Log.CloseAndFlush();
}
