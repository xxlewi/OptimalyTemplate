using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OT.DataLayer.Entities;

namespace OT.DataLayer.Configurations;

/// <summary>
/// Enhanced EF Core configuration for TemplateCategory entity using CRM pattern
/// Template configuration - remove in production or use as reference
/// </summary>
public class TemplateCategoryConfiguration : BaseConfigurableEntityConfiguration<TemplateCategory>
{
    /// <summary>
    /// Configure TemplateCategory-specific properties
    /// </summary>
    public override void ConfigureEntity(EntityTypeBuilder<TemplateCategory> builder)
    {
        // Table name
        builder.ToTable("TemplateCategories");
        
        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.Description)
            .HasMaxLength(500);
            
        builder.Property(c => c.DisplayOrder)
            .HasDefaultValue(0);
            
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
    }
    
    /// <summary>
    /// Configure relationships for TemplateCategory
    /// </summary>
    public override void ConfigureRelationships(EntityTypeBuilder<TemplateCategory> builder)
    {
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent category deletion if has products
    }
    
    /// <summary>
    /// Configure indexes for TemplateCategory performance
    /// </summary>
    public override void ConfigureIndexes(EntityTypeBuilder<TemplateCategory> builder)
    {
        builder.HasIndex(c => c.Name)
            .IsUnique();
            
        builder.HasIndex(c => c.DisplayOrder);
        builder.HasIndex(c => new { c.IsActive, c.DisplayOrder }); // Composite for ordering active categories
    }
    
    /// <summary>
    /// Seed demo data for TemplateCategory
    /// </summary>
    public override void SeedData(EntityTypeBuilder<TemplateCategory> builder)
    {
        var baseDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new TemplateCategory { Id = 1, Name = "Elektronika", Description = "Elektronické zařízení", DisplayOrder = 1, CreatedAt = baseDate },
            new TemplateCategory { Id = 2, Name = "Oblečení", Description = "Módní oblečení", DisplayOrder = 2, CreatedAt = baseDate },
            new TemplateCategory { Id = 3, Name = "Knihy", Description = "Knihy a publikace", DisplayOrder = 3, CreatedAt = baseDate }
        );
    }
}