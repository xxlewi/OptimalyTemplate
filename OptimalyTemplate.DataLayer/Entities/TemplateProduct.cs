using OptimalyTemplate.DataLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OptimalyTemplate.DataLayer.Entities;

/// <summary>
/// Template entity for development reference - demonstrates full CRUD pattern
/// Remove in production or rename to your domain entity
/// </summary>
public class TemplateProduct : BaseEntity
{
    /// <summary>
    /// Product name (required, max 200 chars)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Product description (optional, max 1000 chars)
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Product price (required, positive)
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Sale price (optional, must be less than Price if set)
    /// </summary>
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// Stock quantity
    /// </summary>
    public int StockQuantity { get; set; } = 0;
    
    /// <summary>
    /// SKU/Product code (optional, max 50 chars)
    /// </summary>
    public string? Sku { get; set; }
    
    /// <summary>
    /// Is product active/visible
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Is product featured
    /// </summary>
    public bool IsFeatured { get; set; } = false;
    
    /// <summary>
    /// Category foreign key
    /// </summary>
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Navigation property - product category
    /// </summary>
    public virtual TemplateCategory Category { get; set; } = null!;
    
    /// <summary>
    /// Computed property - effective selling price
    /// </summary>
    public decimal EffectivePrice => SalePrice ?? Price;
    
    /// <summary>
    /// Computed property - is on sale
    /// </summary>
    public bool IsOnSale => SalePrice.HasValue && SalePrice < Price;
    
    /// <summary>
    /// Computed property - discount percentage
    /// </summary>
    public decimal? DiscountPercentage => IsOnSale ? Math.Round((1 - (SalePrice!.Value / Price)) * 100, 2) : null;
    
    /// <summary>
    /// Computed property - stock status
    /// </summary>
    public string StockStatus => StockQuantity switch
    {
        0 => "Vyprodáno",
        <= 5 => "Málo skladem",
        _ => "Skladem"
    };
    
    /// <summary>
    /// Computed property - stock status CSS class for UI styling
    /// </summary>
    public string StockStatusClass => StockQuantity switch
    {
        0 => "text-danger",
        <= 5 => "text-warning", 
        _ => "text-success"
    };
    
    /// <summary>
    /// Computed property - product display name with category
    /// </summary>
    public string DisplayName => $"{Name} ({Category?.Name ?? "Bez kategorie"})";
    
    /// <summary>
    /// Computed property - formatted price with currency
    /// </summary>
    public string FormattedPrice => Price.ToString("C");
    
    /// <summary>
    /// Computed property - formatted effective price with currency
    /// </summary>
    public string FormattedEffectivePrice => EffectivePrice.ToString("C");
    
    /// <summary>
    /// Computed property - sale badge text for UI
    /// </summary>
    public string? SaleBadge => IsOnSale ? $"-{DiscountPercentage:F0}%" : null;
    
    /// <summary>
    /// Computed property - product status for display
    /// </summary>
    public string StatusDisplay => IsActive ? "Aktivní" : "Neaktivní";
    
    /// <summary>
    /// Computed property - product status CSS class
    /// </summary>
    public string StatusClass => IsActive ? "text-success" : "text-muted";
    
    /// <summary>
    /// Computed property - is product low in stock (5 or less)
    /// </summary>
    public bool IsLowStock => StockQuantity <= 5 && StockQuantity > 0;
    
    /// <summary>
    /// Computed property - is product out of stock
    /// </summary>
    public bool IsOutOfStock => StockQuantity == 0;
    
    /// <summary>
    /// Computed property - product summary for lists/dropdowns
    /// </summary>
    public string ProductSummary => $"{Name} - {FormattedEffectivePrice} ({StockStatus})";
    
    /// <summary>
    /// Computed property - featured badge for UI
    /// </summary>
    public string? FeaturedBadge => IsFeatured ? "Featured" : null;
}