using OptimalyTemplate.DataLayer.Interfaces;

namespace OptimalyTemplate.DataLayer.Entities;

/// <summary>
/// Template entity for development reference - demonstrates category/lookup pattern
/// Remove in production or rename to your domain entity
/// </summary>
public class TemplateCategory : BaseEntity
{
    /// <summary>
    /// Category name (required, max 100 chars)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional description
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Category display order
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
    
    /// <summary>
    /// Is category active/visible
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Navigation property - products in this category
    /// </summary>
    public virtual ICollection<TemplateProduct> Products { get; set; } = new List<TemplateProduct>();
    
    /// <summary>
    /// Computed property for display with product count
    /// </summary>
    public string DisplayName => $"{Name} ({Products.Count} produktů)";
    
    /// <summary>
    /// Computed property - active products count
    /// </summary>
    public int ActiveProductsCount => Products.Count(p => p.IsActive);
    
    /// <summary>
    /// Computed property - featured products count
    /// </summary>
    public int FeaturedProductsCount => Products.Count(p => p.IsFeatured && p.IsActive);
    
    /// <summary>
    /// Computed property - products on sale count
    /// </summary>
    public int SaleProductsCount => Products.Count(p => p.IsOnSale && p.IsActive);
    
    /// <summary>
    /// Computed property - out of stock products count
    /// </summary>
    public int OutOfStockCount => Products.Count(p => p.IsOutOfStock);
    
    /// <summary>
    /// Computed property - category status display
    /// </summary>
    public string StatusDisplay => IsActive ? "Aktivní" : "Neaktivní";
    
    /// <summary>
    /// Computed property - category status CSS class
    /// </summary>
    public string StatusClass => IsActive ? "text-success" : "text-muted";
    
    /// <summary>
    /// Computed property - average price in category
    /// </summary>
    public decimal? AveragePrice => Products.Any(p => p.IsActive) 
        ? Products.Where(p => p.IsActive).Average(p => p.EffectivePrice) 
        : null;
    
    /// <summary>
    /// Computed property - lowest price in category
    /// </summary>
    public decimal? LowestPrice => Products.Any(p => p.IsActive) 
        ? Products.Where(p => p.IsActive).Min(p => p.EffectivePrice) 
        : null;
    
    /// <summary>
    /// Computed property - highest price in category  
    /// </summary>
    public decimal? HighestPrice => Products.Any(p => p.IsActive) 
        ? Products.Where(p => p.IsActive).Max(p => p.EffectivePrice) 
        : null;
    
    /// <summary>
    /// Computed property - formatted average price
    /// </summary>
    public string FormattedAveragePrice => AveragePrice?.ToString("C") ?? "N/A";
    
    /// <summary>
    /// Computed property - price range display
    /// </summary>
    public string PriceRange => (LowestPrice, HighestPrice) switch
    {
        (null, null) => "Žádné produkty",
        var (low, high) when low == high => low!.Value.ToString("C"),
        var (low, high) => $"{low:C} - {high:C}"
    };
    
    /// <summary>
    /// Computed property - category summary for display
    /// </summary>
    public string CategorySummary => $"{Name} ({ActiveProductsCount} aktivních, {PriceRange})";
}