# 🚀 OptimalyTemplate

**Modern .NET 9 enterprise application template** with clean 3-layer architecture, PostgreSQL, and AdminLTE UI.

## 🎯 What is this?

OptimalyTemplate is a **production-ready project template** for building scalable .NET web applications. It provides:

- ✅ **Clean 3-Layer Architecture** (Presentation → Service → Data)
- ✅ **Repository & Unit of Work** patterns
- ✅ **PostgreSQL + pgAdmin** Docker setup
- ✅ **AdminLTE 3.2.0** responsive dashboard
- ✅ **AutoMapper** for object mapping
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
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

2. **Add DbSet** to `ApplicationDbContext.cs`:
```csharp
public DbSet<Customer> Customers { get; set; }
```

3. **Create Migration**:
```bash
dotnet ef migrations add AddCustomer --startup-project ../OT.PresentationLayer
```

4. **Create DTO** (`OT.ServiceLayer/DTOs/CustomerDto.cs`):
```csharp
public class CustomerDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

5. **Update AutoMapper** (`OT.ServiceLayer/Mapping/MappingProfile.cs`):
```csharp
CreateMap<Customer, CustomerDto>().ReverseMap();
```

6. **Create Service Interface & Implementation**
7. **Create ViewModel & Controller**
8. **Create Views with AdminLTE styling**

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