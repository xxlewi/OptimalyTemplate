# DataLayer Tests - Princip 20/80

## 📊 Aktuální stav

**Build status:** ✅ Plně funkční (6/6 testů prochází)  
**Poslední update:** 7. srpna 2025  
**Připraveno pro:** Použití jako template pro ostatní projekty

## 🎯 Filozofie testování

**Princip 20/80** - 20% úsilí pro 80% jistoty

### Proč tento přístup?
- ✅ **Rychlé** - běží < 10 sekund
- ✅ **Jednoduché** - jeden soubor, 200 řádků kódu
- ✅ **Udržovatelné** - minimum dependencies
- ✅ **Efektivní** - odhalí většinu kritických problémů
- ✅ **Reálné** - používá skutečný PostgreSQL, ne mocks
- ✅ **CI/CD ready** - běží všude kde je Docker

### Co testujeme (6 testů)

| Test | Status | Popis |
|------|--------|-------|
| `Minimal_CRUD_Works` | ✅ Pass | CRUD operace na entitách |
| `Minimal_SoftDelete_Works` | ✅ Pass | Soft delete s query filtry |
| `Relationships_Work` | ✅ Pass | Vztahy mezi entitami (FK) |
| `BaseEntityFields_AreHandled` | ✅ Pass | Audit fields (CreatedAt, UpdatedBy...) |
| `Concurrency_IsRespected` | ✅ Pass | Souběžný přístup k DB |
| `QueryFilters_ApplyCorrectly` | ✅ Pass | Globální filtry pro soft delete |

## 🏗️ Struktura projektu

```
ODigitalniArchiv.DataLayer.Tests/
├── BasicIntegrationTest.cs    # Všech 6 testů v jednom souboru
├── DebugTest.cs              # Debug testy (lze smazat)
├── README.md                 # Tento soubor
└── *.csproj                  # Projekt s test dependencies
```

### Proč samostatný test projekt?

✅ **Best practice z důvodů:**
- Test dependencies (xUnit, Testcontainers) nejdou do produkce
- Čistší build a deployment
- Oddělení concerns
- .NET standard konvence
- Izolované testovací prostředí

❌ **NEDÁVAT testy do DataLayer projektu:**
- Zvětší produkční DLL
- Security risk
- Porušení Single Responsibility Principle
- Míchání test a produkčního kódu

## 🚀 Jak spustit testy

### Požadavky
- Docker Desktop (pro PostgreSQL container)
- .NET 9.0 SDK

### Příkazy
```bash
# Spustit všechny testy
dotnet test

# Spustit s detaily
dotnet test --logger "console;verbosity=detailed"

# Spustit konkrétní test
dotnet test --filter "Minimal_CRUD_Works"
```

## 📝 Jak psát nové testy

### 1. Držte se vzoru BasicIntegrationTest.cs

```csharp
[Fact]
public async Task NovyTest_CoTestuje_OcekavanyVysledek()
{
    // Arrange - připravit data
    var entita = new Entita { ... };
    
    // Act - provést akci
    await _context.Entity.AddAsync(entita);
    await _context.SaveChangesAsync();
    
    // Assert - ověřit výsledek
    result.Should().NotBeNull();
}
```

### 2. Pravidla pro testy

- **Jeden test = jedna věc** - netestujte všechno najednou
- **Název testu** - `MetodaNeboFeature_Scenario_OcekavanyVysledek`
- **Arrange-Act-Assert** - jasná struktura
- **Používejte FluentAssertions** - `Should().Be()` místo `Assert.Equal()`
- **Async/await vždy** - databázové operace jsou async
- **Skutečná databáze** - PostgreSQL přes Testcontainers

### 3. Co NEtestovat

❌ Gettery/Settery  
❌ Entity Framework Core funkcionalitu  
❌ Jednoduché LINQ queries  
❌ Konfiguraci a mappingy  
❌ Automaticky generovaný kód

### 4. Co testovat

✅ Repository metody s business logikou  
✅ Složité queries a filtry  
✅ Soft delete a audit funkce  
✅ Custom validace  
✅ Vztahy mezi entitami
✅ Souběžný přístup

## 🔧 Vyřešené problémy

1. **SQL Server syntax v PostgreSQL** - opraveny check constrainty s `[]` na `""`
2. **Template konfigurace** - vyloučeny z ApplicationDbContext
3. **Array.Empty<byte>()** - změněno na `new byte[0]` pro PostgreSQL

## 📈 Budoucí vylepšení

Přidat testy **POUZE když**:
- Najdete bug v produkci → test který ho odhalí
- Přidáte kritickou funkcionalitu → jeden test
- Máte opakující se problém → test jako prevence

## 🎓 Použití jako template

Tento přístup můžete zkopírovat do ostatních projektů:

1. Zkopírujte `BasicIntegrationTest.cs`
2. Upravte namespace a DbContext
3. Spusťte testy
4. Postupně přidávejte podle potřeby

## 🐳 Testcontainers

Testy používají **Testcontainers** - automaticky spouští PostgreSQL v Docker kontejneru:
- Každý test má vlastní čistou databázi
- Automatický cleanup po testech
- Žádná konfigurace nebo instalace PostgreSQL
- Funguje lokálně i v CI/CD

## 💡 Závěr

> "Lepší mít 6 fungujících testů než 600 nefungujících"

Tento minimalistický přístup vám dá jistotu že DataLayer funguje, bez overhead složité test architektury. 

**Výhody:**
- Testuje proti skutečné databázi (PostgreSQL)
- Pokrývá klíčové scénáře
- Rychlé a spolehlivé
- Snadná údržba

Začněte jednoduše, rostěte podle potřeby.