# 🚀 OptimalyTemplate

**Modern .NET 9 enterprise application template** with clean 3-layer architecture, PostgreSQL, and AdminLTE UI.

## 🎯 What is this?

OptimalyTemplate is a **production-ready project template** for building scalable .NET web applications. It provides:

- ✅ **Clean 3-Layer Architecture** (Presentation → Service → Data)
- ✅ **Repository & Unit of Work** patterns
- ✅ **PostgreSQL + pgAdmin** Docker setup
- ✅ **AdminLTE 3.2.0** responsive dashboard
- ✅ **AutoMapper** for object mapping
- ✅ **Serilog structured logging** with file and console output
- ✅ **Global error handling** middleware with custom exceptions
- ✅ **Health checks** for application, database and PostgreSQL monitoring
- ✅ **Dynamic configuration** system for easy project forking
- ✅ **VS Code integration** with F5 debugging

Perfect for **enterprise applications**, **microservices**, or any project requiring solid architectural foundations.

## 🚀 Quick Start (5 Minutes)

### 1. Fork & Clone
```bash
git clone https://github.com/xxlewi/OptimalyTemplate.git
cd OptimalyTemplate
```

### 2. Customize Your Project
Edit `Directory.Build.props`:
```xml
<AppName>MyAwesomeProject</AppName>          <!-- Your project name -->
<DockerPostgresPort>5435</DockerPostgresPort> <!-- Unique port -->
```

### 3. Generate Docker Configuration
```bash
# Linux/macOS
./generate-docker-config.sh

# Windows
.\generate-docker-config.ps1
```

### 4. Start Database
```bash
docker-compose -f docker-compose.generated.yml up -d
```

### 5. Update Connection String
Copy the generated connection string to `OT.PresentationLayer/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5435;Database=MyAwesomeProject_db;Username=MyAwesomeProject_user;Password=MyAwesomeProject2024!"
  }
}
```

### 6. Run Migrations & Start App
```bash
cd OT.DataLayer
dotnet ef database update --startup-project ../OT.PresentationLayer

cd ../OT.PresentationLayer
dotnet run
```

**🎉 Done!** Your app is running with:
- **Web App**: http://localhost:5000
- **pgAdmin**: http://localhost:5051 (admin@yourlowerappname.local / admin123)

---

## 📚 Documentation

| Topic | Description |
|-------|-------------|
| [🏗️ **Architecture & Infrastructure**](README-INFRA.md) | Detailed architecture, patterns, and technical implementation |
| [🐳 **Docker Setup**](README-DOCKER.md) | Docker configuration, ports, and database management |

## 🎯 Why Use This Template?

### ✅ **Production Ready**
- Security audit passed
- Clean architecture verified
- Industry best practices
- Enterprise-grade patterns

### 🔧 **Developer Friendly**
- VS Code F5 debugging configured
- Auto-generated Docker setup
- AdminLTE dashboard included
- Easy project customization

### 🏗️ **Solid Architecture**
- **Presentation Layer**: Controllers, ViewModels, AdminLTE Views
- **Service Layer**: Business logic, DTOs, AutoMapper
- **Data Layer**: Entities, Repository, Unit of Work, EF Core

### 🔄 **Easy Forking**
1. Change app name in one file
2. Run generation script
3. Start coding your features

## 🛠️ What's Included

```
OptimalyTemplate/
├── 🎨 AdminLTE 3.2.0 UI Framework
├── 🗄️ PostgreSQL + pgAdmin Docker Setup  
├── 🏗️ Clean 3-Layer Architecture
├── 🔄 Repository & Unit of Work Patterns
├── 🗺️ AutoMapper Configuration
├── 📊 Serilog Structured Logging
├── 🛡️ Global Error Handling Middleware
├── 💓 Health Checks & Monitoring
├── 🔐 Security Best Practices
├── 📝 VS Code Debug Configuration
├── 🚀 Dynamic Project Generation
└── 📚 Comprehensive Documentation
```

## 🔧 Technologies

- **.NET 9** - Latest .NET framework
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core** - ORM with PostgreSQL
- **AutoMapper** - Object mapping
- **AdminLTE 3.2.0** - Admin dashboard template
- **PostgreSQL 16** - Database
- **Docker** - Containerization
- **Bootstrap 4** - CSS framework

## 📋 Creating New Features

### Adding a New Entity (e.g., Customer)

1. **Create Entity** (`OT.DataLayer/Entities/Customer.cs`):
```csharp
namespace OT.DataLayer.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    
    public string FullName => $"{FirstName} {LastName}".Trim();
}
```

2. **Create Entity Configuration** (`OT.DataLayer/Configurations/CustomerConfiguration.cs`):
```csharp
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(50);
        
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
```

3. **Add DbSet** to `ApplicationDbContext.cs`:
```csharp
public DbSet<Customer> Customers { get; set; }
```

4. **Create Migration**:
```bash
cd OT.DataLayer
dotnet ef migrations add AddCustomer --startup-project ../OT.PresentationLayer
dotnet ef database update --startup-project ../OT.PresentationLayer
```

5. **Create DTO** (`OT.ServiceLayer/DTOs/CustomerDto.cs`):
```csharp
namespace OT.ServiceLayer.DTOs;

public class CustomerDto : BaseDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string FullName { get; set; } = string.Empty;
}
```

6. **Update AutoMapper** (`OT.ServiceLayer/Mapping/MappingProfile.cs`):
```csharp
CreateMap<Customer, CustomerDto>().ReverseMap();
```

7. **Create Service Interface** (`OT.ServiceLayer/Interfaces/ICustomerService.cs`):
```csharp
public interface ICustomerService : IBaseService<CustomerDto>
{
    Task<IEnumerable<CustomerDto>> GetByEmailAsync(string email);
}
```

8. **Create Service Implementation** (`OT.ServiceLayer/Services/CustomerService.cs`):
```csharp
public class CustomerService : BaseService<Customer, CustomerDto>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper) { }
        
    public async Task<IEnumerable<CustomerDto>> GetByEmailAsync(string email)
    {
        // Custom business logic here
    }
}
```

9. **Register Service** in `ServiceCollectionExtensions.cs`:
```csharp
services.AddScoped<ICustomerService, CustomerService>();
```

10. **Create ViewModel, Controller & Views** with AdminLTE styling

## 📊 Logging & Error Handling

### Serilog Configuration
- **Structured logging** with JSON output
- **File rotation** (daily, 30 days retention)
- **Request logging** with performance metrics
- **Environment-based** log levels

Logs are saved to `logs/` directory:
```
logs/log-20250729.txt
logs/log-20250730.txt
...
```

### Global Exception Handling
Custom exceptions with proper HTTP status codes:

```csharp
// Not Found (404)
throw new NotFoundException("Customer", customerId);

// Validation Error (400)
throw new ValidationException("Email", "Email is required");

// Business Logic Error (400)
throw new BusinessException("Cannot delete active customer", "ACTIVE_CUSTOMER");
```

### Test Endpoints
Test error handling at:
- `GET /api/test/test-not-found` - 404 Error
- `GET /api/test/test-validation` - 400 Validation Error
- `GET /api/test/test-business` - 400 Business Error
- `GET /api/test/test-general` - 500 Internal Error
- `GET /api/test/test-success` - 200 Success

## 💓 Health Checks & Monitoring

### Health Check Endpoints
- **`GET /health`** - Detailní JSON report všech health checks
- **`GET /health/ready`** - Jednoduchá kontrola pro load balancery
- **`GET /health` (UI)** - AdminLTE dashboard pro health monitoring

### Implementované kontroly
1. **Application Check** - Stav aplikace, verze, paměť, uptime
2. **Database Check** - Entity Framework DbContext připojení
3. **PostgreSQL Check** - Přímé připojení k PostgreSQL databázi

### Příklad JSON response:
```json
{
  "status": "Healthy",
  "totalDuration": 45.2,
  "checks": [
    {
      "name": "application",
      "status": "Healthy",
      "description": "Aplikace běží správně",
      "duration": 2.1,
      "data": {
        "version": "1.0.0",
        "environment": "Development",
        "uptime": 1234.5
      }
    }
  ]
}
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch
3. Follow the existing architecture patterns
4. Update documentation if needed
5. Submit a pull request

## 📄 License

This template is open source and available under the MIT License.

---

**Ready to build something awesome?** 🚀 Start with this template and focus on your business logic instead of boilerplate code!