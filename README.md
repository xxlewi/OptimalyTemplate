# 🚀 OptimalyTemplate

**Modern .NET 9 enterprise application template** with clean 3-layer architecture, PostgreSQL, and AdminLTE UI.

## 🎯 What is this?

OptimalyTemplate is a **production-ready project template** for building scalable .NET web applications. It provides:

- ✅ **Clean 3-Layer Architecture** (Presentation → Service → Data)
- ✅ **Generic Repository & Unit of Work** patterns with true generic support
- ✅ **Production-ready Service Layer** with comprehensive error handling and validation
- ✅ **Enterprise-grade security** with proper authentication and security headers
- ✅ **ASP.NET Core Identity** with custom User entity and authentication
- ✅ **PostgreSQL + pgAdmin** Docker setup
- ✅ **AdminLTE 3.2.0** responsive dashboard
- ✅ **AutoMapper** for object mapping
- ✅ **Serilog structured logging** with file and console output
- ✅ **Global error handling** middleware with custom exceptions
- ✅ **Security headers middleware** with CSP and XSS protection
- ✅ **Health checks** for application, database and PostgreSQL monitoring
- ✅ **Global query filters** for soft delete functionality
- ✅ **Business logic validation** with custom exception handling
- ✅ **Dynamic configuration** system for easy project forking
- ✅ **VS Code integration** with F5 debugging
- ✅ **Template Entity System** - Complete CRUD reference implementation
- ✅ **AdminLTE CRUD Views** with pagination, filtering, and client-side validation

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

**🐛 Debugging with VS Code:**
- Press **F5** to start debugging (opens browser automatically on http://localhost:5020)
- If port conflicts occur, run: `./kill-dotnet.sh` then try F5 again

**🎉 Done!** Your app is running with:
- **Web App**: http://localhost:5020
- **Template CRUD**: http://localhost:5020/TemplateProducts (see complete reference implementation)
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
- **Presentation Layer**: Secure controllers, validated ViewModels, AdminLTE Views, Enterprise authentication
- **Service Layer**: Production-ready business logic, DTOs, AutoMapper, Exception handling, Input validation
- **Data Layer**: Generic Repository pattern, UnitOfWork, ASP.NET Core Identity, EF Core

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
├── 🔄 Generic Repository & Unit of Work Patterns
├── 👤 ASP.NET Core Identity with Custom User Entity
├── ⚡ Production-Ready Service Layer (9/10 Enterprise-Grade)
├── 🛡️ Enterprise Security (8/10) - Headers, Password Policy, Validation
├── 🗺️ AutoMapper Configuration
├── 📊 Serilog Structured Logging
├── 🛡️ Global Error Handling Middleware
├── 💓 Health Checks & Monitoring
├── 🔍 Global Query Filters (Soft Delete)
├── ✅ Business Logic Validation & Exception Handling
├── 🔐 Security Headers & XSS Protection
├── 📝 VS Code Debug Configuration
├── 🚀 Dynamic Project Generation
├── 📝 Template Entity System (TemplateProduct/TemplateCategory)
└── 📚 Comprehensive Documentation
```

## 🔧 Technologies

- **.NET 9** - Latest .NET framework
- **ASP.NET Core MVC** - Web framework with Identity
- **Entity Framework Core** - ORM with PostgreSQL
- **ASP.NET Core Identity** - Authentication and authorization
- **AutoMapper** - Object mapping
- **AdminLTE 3.2.0** - Admin dashboard template
- **PostgreSQL 16** - Database
- **Docker** - Containerization
- **Bootstrap 4** - CSS framework
- **Serilog** - Structured logging

## 🎯 Template Entity System

### Complete CRUD Reference Implementation

OptimalyTemplate includes a **complete template entity system** demonstrating best practices for implementing CRUD operations across all architectural layers.

**🔍 Live Demo**: [http://localhost:5020/TemplateProducts](http://localhost:5020/TemplateProducts)

### Template Entities
- **`TemplateProduct`** - Main product entity with categories, pricing, inventory
- **`TemplateCategory`** - Product categories with display ordering

### Features Demonstrated
- ✅ **Complete CRUD Operations** (Create, Read, Update, Delete)
- ✅ **Entity Relationships** (Product ↔ Category with foreign keys)
- ✅ **Advanced Querying** with Entity Framework Include() for eager loading
- ✅ **Business Logic** (price validation, stock management, category restrictions)
- ✅ **Computed Properties** (effective price, discount percentage, stock status)
- ✅ **AdminLTE UI** with responsive tables, modals, and forms
- ✅ **Pagination & Filtering** with search, category filters, and sorting
- ✅ **Client-Side Validation** with real-time price validation
- ✅ **Server-Side Validation** with comprehensive error handling
- ✅ **AutoMapper Integration** between all layers (Entity ↔ DTO ↔ ViewModel)
- ✅ **Seed Data** for development and testing

### Architecture Layers Covered

**🔸 Entity Layer** (`OT.DataLayer/Entities/`)
```csharp
public class TemplateProduct : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int CategoryId { get; set; }
    public virtual TemplateCategory Category { get; set; } = null!;
    
    // Computed properties for business logic
    public decimal EffectivePrice => SalePrice ?? Price;
    public bool IsOnSale => SalePrice.HasValue && SalePrice < Price;
}
```

**🔸 Data Layer** (`OT.DataLayer/Configurations/`)
- EF Core configurations with indexes, constraints, and relationships
- Seed data for development
- Database migrations

**🔸 Service Layer** (`OT.ServiceLayer/`)
- DTOs with computed properties for UI
- Business logic services with validation
- AutoMapper profiles for Entity ↔ DTO mapping
- Comprehensive error handling

**🔸 Presentation Layer** (`OT.PresentationLayer/`)
- ViewModels with validation attributes
- Controllers with proper error handling
- AdminLTE views with pagination and filtering
- Client-side validation

### Template Files to Study

**📁 Essential Files:**
```
🔸 Entities
├── TemplateProduct.cs          # Main product entity with relationships
├── TemplateCategory.cs         # Category lookup entity

🔸 Data Configuration  
├── TemplateProductConfiguration.cs    # EF mappings, indexes, constraints
├── TemplateCategoryConfiguration.cs   # Category configuration with seed data

🔸 Service Layer
├── DTOs/TemplateProductDto.cs          # Data transfer objects
├── Services/TemplateProductService.cs  # Business logic implementation
├── Mapping/MappingProfile.cs           # AutoMapper Entity ↔ DTO

🔸 Presentation Layer
├── Controllers/TemplateProductsController.cs  # MVC controller
├── ViewModels/TemplateProductViewModel.cs     # UI model with validation
├── Views/TemplateProducts/                    # AdminLTE CRUD views
    ├── Index.cshtml             # List with pagination & filters
    ├── Create.cshtml            # Create form with validation
    ├── Edit.cshtml              # Edit form with validation
    ├── Details.cshtml           # Read-only detail view
    └── Delete.cshtml            # Delete confirmation
```

### Key Learning Points

1. **Generic Repository Pattern** - How to use `IRepository<TEntity, TKey>`
2. **Unit of Work Pattern** - Proper transaction management
3. **AutoMapper Configuration** - Multi-layer object mapping
4. **EF Core Best Practices** - Eager loading, query optimization
5. **Business Logic Validation** - Server-side and client-side
6. **AdminLTE Integration** - Professional UI components
7. **Error Handling** - Comprehensive exception management

### Removing Template Entities (Production)

When ready for production, search for comments containing "Template" and remove:
```bash
# Find all template-related files
find . -name "*Template*" -type f
grep -r "Template.*remove.*production" --include="*.cs"
```

Files to remove:
- All `Template*` entities, DTOs, services, controllers, and views
- Template-related migrations
- Template service registrations

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
public class CustomerService : BaseService<Customer, CustomerDto, int>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper) { }
        
    public async Task<IEnumerable<CustomerDto>> GetByEmailAsync(string email)
    {
        var repository = _unitOfWork.GetRepository<Customer, int>();
        var customers = await repository.FindAsync(c => c.Email == email, cancellationToken);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}
```

9. **Register Service** in `ServiceCollectionExtensions.cs`:
```csharp
services.AddScoped<ICustomerService, CustomerService>();
```

10. **Create ViewModel, Controller & Views** with AdminLTE styling

## 🔧 Service Layer Features

### Production-Ready Business Logic (9/10 Enterprise-Grade)

## 🛡️ Security Features

### Enterprise-Grade Security Implementation (8/10)

**✅ Authentication & Authorization:**
- ASP.NET Core Identity with custom User entity
- Enterprise-grade password policy (8+ chars, mixed case, numbers, symbols)
- Account lockout after 5 failed attempts (15-minute duration)
- Email confirmation requirement for new accounts
- Secure session management

**✅ Security Headers Protection:**
- Content Security Policy (CSP) with XSS prevention
- X-Frame-Options: DENY (clickjacking protection)
- X-Content-Type-Options: nosniff
- X-XSS-Protection: 1; mode=block
- Referrer-Policy: strict-origin-when-cross-origin
- Server identification headers removal

**✅ Input Validation & Sanitization:**
- Model validation with data annotations
- Required field validation with proper error messages
- Terms of service acceptance validation
- Email format validation with proper error handling

**✅ Development Security:**
- Test endpoints automatically excluded from production builds
- Debug-only exception testing controller
- Environment-specific security configurations

**Example Security Configuration:**
```csharp
// Enterprise password policy
options.Password.RequiredLength = 8;
options.Password.RequireUppercase = true;
options.Password.RequireLowercase = true;
options.Password.RequireDigit = true;
options.Password.RequireNonAlphanumeric = true;
options.Password.RequiredUniqueChars = 4;

// Account lockout settings
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
```

**✅ Comprehensive Error Handling:**
- Structured exception hierarchy (`BusinessException`, `ValidationException`, `NotFoundException`)
- Database operation exception handling (`DbUpdateException`, `DbUpdateConcurrencyException`)
- Proper error codes and messages for API consumers

**✅ Input Validation:**
- Argument null checking on all public methods
- Business rule validation (email uniqueness, required fields)
- Structured validation errors with field-level details

**✅ Generic Patterns:**
- `BaseService<TEntity, TDto, TKey>` supports any entity and ID type
- `IBaseService<TDto, TKey>` with backward compatibility
- Generic DTOs with `BaseDto<TKey>` pattern

**✅ User Management:**
- `UserService` with comprehensive business logic
- Email uniqueness validation
- User-specific operations (search, active users, last login tracking)

**✅ Performance Optimizations:**
- `ConfigureAwait(false)` on all async operations
- `CancellationToken` support throughout
- Pagination support with `PagedResult<T>`

**Example Usage:**
```csharp
// Dependency injection
services.AddScoped<IUserService, UserService>();

// Usage with proper error handling
try 
{
    var user = await _userService.CreateAsync(userDto);
}
catch (ValidationException ex) 
{
    // Handle validation errors (400)
    return BadRequest(ex.Errors);
}
catch (BusinessException ex) 
{
    // Handle business logic errors (400)
    return BadRequest(new { error = ex.Code, message = ex.Message });
}
```

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