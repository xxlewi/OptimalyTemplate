using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OT.DataLayer.Entities;

namespace OT.DataLayer.Configurations;

/// <summary>
/// EF Core configuration for TemplateCategory entity
/// Template configuration - remove in production
/// </summary>
public class TemplateCategoryConfiguration : IEntityTypeConfiguration<TemplateCategory>
{
    public void Configure(EntityTypeBuilder<TemplateCategory> builder)
    {
        // Table name
        builder.ToTable("TemplateCategories");
        
        // Primary key
        builder.HasKey(c => c.Id);
        
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
        
        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();
            
        builder.HasIndex(c => c.DisplayOrder);
        
        // Relationships
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent category deletion if has products
        
        // Seed data
        builder.HasData(
            new TemplateCategory { Id = 1, Name = "Elektronika", Description = "Elektronické zařízení", DisplayOrder = 1, CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new TemplateCategory { Id = 2, Name = "Oblečení", Description = "Módní oblečení", DisplayOrder = 2, CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new TemplateCategory { Id = 3, Name = "Knihy", Description = "Knihy a publikace", DisplayOrder = 3, CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}