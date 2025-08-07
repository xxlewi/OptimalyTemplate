using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OptimalyTemplate.DataLayer.Entities;

namespace OptimalyTemplate.DataLayer.Configurations;

/// <summary>
/// Enhanced EF Core configuration for TemplateProduct entity using CRM pattern
/// Template configuration - remove in production or use as reference
/// </summary>
public class TemplateProductConfiguration : BaseConfigurableEntityConfiguration<TemplateProduct>
{
    /// <summary>
    /// Configure TemplateProduct-specific properties
    /// </summary>
    public override void ConfigureEntity(EntityTypeBuilder<TemplateProduct> builder)
    {
        // Table name
        builder.ToTable("TemplateProducts");
        
        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
            
        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
            
        builder.Property(p => p.SalePrice)
            .HasColumnType("decimal(18,2)");
            
        builder.Property(p => p.StockQuantity)
            .HasDefaultValue(0);
            
        builder.Property(p => p.Sku)
            .HasMaxLength(50);
            
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
            
        builder.Property(p => p.IsFeatured)
            .HasDefaultValue(false);
        
        // Check constraints - using new API for EF Core 8+
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_TemplateProduct_Price_Positive", "\"Price\" > 0");
            t.HasCheckConstraint("CK_TemplateProduct_SalePrice_Positive", "\"SalePrice\" IS NULL OR \"SalePrice\" > 0");
            t.HasCheckConstraint("CK_TemplateProduct_SalePrice_LessThanPrice", "\"SalePrice\" IS NULL OR \"SalePrice\" < \"Price\"");
            t.HasCheckConstraint("CK_TemplateProduct_StockQuantity_NonNegative", "\"StockQuantity\" >= 0");
        });
    }
    
    /// <summary>
    /// Configure relationships for TemplateProduct
    /// </summary>
    public override void ConfigureRelationships(EntityTypeBuilder<TemplateProduct> builder)
    {
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    /// <summary>
    /// Configure indexes for TemplateProduct performance
    /// </summary>
    public override void ConfigureIndexes(EntityTypeBuilder<TemplateProduct> builder)
    {
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Sku)
            .IsUnique()
            .HasFilter("\"Sku\" IS NOT NULL"); // Unique only for non-null values
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsFeatured);
        builder.HasIndex(p => new { p.IsActive, p.Price }); // Composite index for active products by price
        builder.HasIndex(p => new { p.CategoryId, p.IsActive }); // Composite for category filtering
    }
    
    /// <summary>
    /// Seed demo data for TemplateProduct
    /// </summary>
    public override void SeedData(EntityTypeBuilder<TemplateProduct> builder)
    {
        // Seed data removed for testing
        // Uncomment to add seed data
        /*
        var baseDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new TemplateProduct 
            { 
                Id = 1, 
                Name = "iPhone 15 Pro", 
                Description = "Nejnovější iPhone s titanovým designem", 
                Price = 32990m, 
                SalePrice = 29990m,
                StockQuantity = 15,
                Sku = "IPH15PRO",
                CategoryId = 1, 
                IsFeatured = true,
                CreatedAt = baseDate 
            },
            new TemplateProduct 
            { 
                Id = 2, 
                Name = "Samsung Galaxy S24", 
                Description = "Pokročilý Android smartphone", 
                Price = 24990m,
                StockQuantity = 8,
                Sku = "SGS24",
                CategoryId = 1,
                CreatedAt = baseDate 
            },
            new TemplateProduct 
            { 
                Id = 3, 
                Name = "Pánská košile", 
                Description = "Elegantní bavlněná košile", 
                Price = 1290m,
                StockQuantity = 25,
                Sku = "SHIRT001",
                CategoryId = 2,
                CreatedAt = baseDate 
            },
            new TemplateProduct 
            { 
                Id = 4, 
                Name = "Čistý kód", 
                Description = "Kniha o psaní kvalitního kódu", 
                Price = 590m,
                StockQuantity = 0, // Out of stock
                Sku = "BOOK001",
                CategoryId = 3,
                CreatedAt = baseDate 
            },
            new TemplateProduct 
            { 
                Id = 5, 
                Name = "Dámské šaty", 
                Description = "Letní šaty v moderním stylu", 
                Price = 2490m,
                SalePrice = 1990m,
                StockQuantity = 3, // Low stock
                Sku = "DRESS001",
                CategoryId = 2,
                IsFeatured = true,
                CreatedAt = baseDate 
            }
        );
        */
    }
}