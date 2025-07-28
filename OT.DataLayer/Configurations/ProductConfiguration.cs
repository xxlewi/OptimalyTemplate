using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OT.DataLayer.Entities;

namespace OT.DataLayer.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(p => p.Description)
            .HasMaxLength(500);
            
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
            
        builder.Property(p => p.CreatedAt)
            .IsRequired();
            
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}