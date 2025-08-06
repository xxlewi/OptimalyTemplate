namespace OptimalyTemplate.ServiceLayer.DTOs;

/// <summary>
/// DTO for TemplateProduct entity
/// Template DTO - remove in production
/// </summary>
public class TemplateProductDto : BaseDto
{
    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Product description
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Product price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Sale price (optional)
    /// </summary>
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// Stock quantity
    /// </summary>
    public int StockQuantity { get; set; }
    
    /// <summary>
    /// Product SKU
    /// </summary>
    public string? Sku { get; set; }
    
    /// <summary>
    /// Is product active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Is product featured
    /// </summary>
    public bool IsFeatured { get; set; }
    
    /// <summary>
    /// Category ID
    /// </summary>
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Category name (for display)
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Effective selling price
    /// </summary>
    public decimal EffectivePrice => SalePrice ?? Price;
    
    /// <summary>
    /// Is product on sale
    /// </summary>
    public bool IsOnSale => SalePrice.HasValue && SalePrice < Price;
    
    /// <summary>
    /// Discount percentage
    /// </summary>
    public decimal? DiscountPercentage => IsOnSale ? Math.Round((1 - (SalePrice!.Value / Price)) * 100, 2) : null;
    
    /// <summary>
    /// Stock status description
    /// </summary>
    public string StockStatus => StockQuantity switch
    {
        0 => "Vyprodáno",
        <= 5 => "Málo skladem",
        _ => "Skladem"
    };
    
    /// <summary>
    /// Stock status CSS class for styling
    /// </summary>
    public string StockStatusClass => StockQuantity switch
    {
        0 => "text-danger",
        <= 5 => "text-warning",
        _ => "text-success"
    };
    
    /// <summary>
    /// Formatted price with currency
    /// </summary>
    public string FormattedPrice => $"{Price:C}";
    
    /// <summary>
    /// Formatted sale price with currency
    /// </summary>
    public string? FormattedSalePrice => SalePrice?.ToString("C");
    
    /// <summary>
    /// Formatted effective price with currency
    /// </summary>
    public string FormattedEffectivePrice => $"{EffectivePrice:C}";
}