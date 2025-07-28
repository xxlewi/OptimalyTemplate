# OptimalyTemplate

3-vrstvá .NET 9 aplikace template s clean architecture.

## Struktura

- **OT.DataLayer** - Entity Framework, entities, repositories, unit of work
- **OT.ServiceLayer** - Business logika, DTOs, services, AutoMapper
- **OT.PresentationLayer** - MVC aplikace, controllers, ViewModels

## Klíčové funkce

- Clean Architecture
- Repository Pattern + Unit of Work
- Base Entity s audit fieldy
- Soft delete
- AutoMapper pro mapování Entity → DTO → ViewModel
- Extension methods pro DI registraci
- Directory.Build.props pro snadné přejmenování

## Jak forknout a přejmenovat

1. **Změňte v Directory.Build.props:**
   ```xml
   <ProjectPrefix>VašPrefix</ProjectPrefix>
   <AppName>VašeAplikace</AppName>
   ```

2. **Přejmenujte projekty a složky:**
   - `OT.DataLayer` → `VašPrefix.DataLayer`
   - `OT.ServiceLayer` → `VašPrefix.ServiceLayer` 
   - `OT.PresentationLayer` → `VašPrefix.PresentationLayer`

3. **Upravte namespace ve všech souborech** (Find & Replace):
   - `OT.DataLayer` → `VašPrefix.DataLayer`
   - `OT.ServiceLayer` → `VašPrefix.ServiceLayer`
   - `OT.PresentationLayer` → `VašPrefix.PresentationLayer`

4. **Změňte connection string v appsettings.json**

## Použití

### Přidání nové entity

1. **DataLayer**: Vytvořte entitu dědící z `BaseEntity`
2. **DataLayer**: Přidejte konfiguraci entity
3. **ServiceLayer**: Vytvořte DTO dědící z `BaseDto`
4. **ServiceLayer**: Vytvořte service interface a implementaci
5. **ServiceLayer**: Přidejte mapování do `MappingProfile`
6. **PresentationLayer**: Vytvořte ViewModel dědící z `BaseViewModel`
7. **PresentationLayer**: Přidejte mapování do `ViewModelMappingProfile`
8. **PresentationLayer**: Vytvořte controller

### Registrace služeb

Služby se registrují automaticky prostřednictvím extension methods v každé vrstvě.

## Konvence mapování

- **Entity (DataLayer)** → **DTO (ServiceLayer)** → **ViewModel (PresentationLayer)**
- AutoMapper profily v každé vrstvě
- BaseEntity → BaseDto → BaseViewModel

## Databáze

Template je nakonfigurován pro SQL Server LocalDB. Změňte connection string v `appsettings.json` podle potřeby.