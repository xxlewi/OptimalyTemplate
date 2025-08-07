using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OptimalyTemplate.DataLayer.Data;
using OptimalyTemplate.DataLayer.Entities;
using Testcontainers.PostgreSql;

namespace OptimalyTemplate.DataLayer.Tests;

public class BasicIntegrationTest : IAsyncLifetime
{
    private PostgreSqlContainer _postgresContainer = null!;
    private ApplicationDbContext _context = null!;

    [Fact]
    public async Task Minimal_CRUD_Works()
    {
        // CREATE
        var category = new TemplateCategory
        {
            Name = "Test Category",
            Description = "Test Description"
        };
        
        await _context.TemplateCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        // READ
        var saved = await _context.TemplateCategories.FindAsync(category.Id);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Test Category");
        
        // UPDATE
        saved.Name = "Updated";
        await _context.SaveChangesAsync();
        
        // DELETE
        _context.TemplateCategories.Remove(saved);
        await _context.SaveChangesAsync();
        
        var deleted = await _context.TemplateCategories.FindAsync(category.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task Minimal_SoftDelete_Works()
    {
        // Create category first for FK
        var category = new TemplateCategory
        {
            Name = "Test Category",
            Description = "Test"
        };
        await _context.TemplateCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        var product = new TemplateProduct
        {
            Name = "Test Product",
            Description = "Test",
            Price = 100,
            CategoryId = category.Id
        };
        
        await _context.TemplateProducts.AddAsync(product);
        await _context.SaveChangesAsync();
        
        // Soft delete
        product.IsDeleted = true;
        await _context.SaveChangesAsync();
        
        // Should be filtered out
        var result = await _context.TemplateProducts.Where(p => p.Id == product.Id).FirstOrDefaultAsync();
        result.Should().BeNull();
        
        // But still exists
        var exists = await _context.TemplateProducts.IgnoreQueryFilters().AnyAsync(p => p.Id == product.Id);
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task Relationships_Work()
    {
        // Create parent
        var category = new TemplateCategory
        {
            Name = "Test Category",
            Description = "Test"
        };
        await _context.TemplateCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        // Create child
        var product = new TemplateProduct
        {
            CategoryId = category.Id,
            Name = "Test Product",
            Description = "Test",
            Price = 100
        };
        await _context.TemplateProducts.AddAsync(product);
        await _context.SaveChangesAsync();
        
        // Verify relationship
        var loadedProduct = await _context.TemplateProducts
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == product.Id);
        
        loadedProduct.Category.Should().NotBeNull();
        loadedProduct.Category.Name.Should().Be("Test Category");
    }

    [Fact]
    public async Task BaseEntityFields_AreHandled()
    {
        var testTime = DateTime.UtcNow;
        
        var category = new TemplateCategory
        {
            Name = "Test Category",
            Description = "Test",
            CreatedAt = testTime,
            CreatedBy = "test-user"
        };
        
        await _context.TemplateCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        // Reload
        var loaded = await _context.TemplateCategories.FindAsync(category.Id);
        
        loaded.Should().NotBeNull();
        loaded!.Id.Should().BeGreaterThan(0);
        loaded.CreatedAt.Should().BeCloseTo(testTime, TimeSpan.FromSeconds(1));
        loaded.CreatedBy.Should().Be("test-user");
        loaded.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task Concurrency_IsRespected()
    {
        // Create category first for FK
        var category = new TemplateCategory
        {
            Name = "Test Category",
            Description = "Test"
        };
        await _context.TemplateCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        var product = new TemplateProduct
        {
            Name = "Original",
            Description = "Test",
            Price = 100,
            CategoryId = category.Id
        };
        
        await _context.TemplateProducts.AddAsync(product);
        await _context.SaveChangesAsync();
        
        // Simulate concurrent updates
        var context1 = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options);
        var context2 = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options);
        
        var product1 = await context1.TemplateProducts.FindAsync(product.Id);
        var product2 = await context2.TemplateProducts.FindAsync(product.Id);
        
        product1!.Name = "Updated1";
        await context1.SaveChangesAsync();
        
        product2!.Name = "Updated2";
        
        // Second update should work (optimistic concurrency not configured)
        await context2.SaveChangesAsync();
        
        // Verify last write wins - need fresh context to see updates
        var context3 = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options);
        var final = await context3.TemplateProducts.FindAsync(product.Id);
        final!.Name.Should().Be("Updated2");
        await context3.DisposeAsync();
        
        await context1.DisposeAsync();
        await context2.DisposeAsync();
    }

    [Fact]
    public async Task QueryFilters_ApplyCorrectly()
    {
        // Create mix of deleted and active
        var categories = new[]
        {
            new TemplateCategory { Name = "Active 1", Description = "Test", IsDeleted = false },
            new TemplateCategory { Name = "Deleted", Description = "Test", IsDeleted = true },
            new TemplateCategory { Name = "Active 2", Description = "Test", IsDeleted = false }
        };
        
        await _context.TemplateCategories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
        
        // Normal query should only return active
        var active = await _context.TemplateCategories.ToListAsync();
        active.Should().HaveCount(2);
        active.Should().NotContain(c => c.IsDeleted);
        
        // With IgnoreQueryFilters should return all
        var all = await _context.TemplateCategories.IgnoreQueryFilters().ToListAsync();
        all.Should().HaveCount(3);
        all.Should().Contain(c => c.IsDeleted);
    }

    public async Task InitializeAsync()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .Build();

        await _postgresContainer.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}