namespace OT.ServiceLayer.DTOs;

/// <summary>
/// DTO for TemplateCategory entity
/// Template DTO - remove in production
/// </summary>
public class TemplateCategoryDto : BaseDto
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Category description
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Is category active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Number of products in category (computed)
    /// </summary>
    public int ProductCount { get; set; }
    
    /// <summary>
    /// Display name with product count
    /// </summary>
    public string DisplayName => $"{Name} ({ProductCount} produktů)";
    
    /// <summary>
    /// Number of active products in category (computed)
    /// </summary>
    public int ActiveProductsCount { get; set; }
    
    /// <summary>
    /// Price range of products in category
    /// </summary>
    public string PriceRange { get; set; } = "N/A";
    
    /// <summary>
    /// Status display text
    /// </summary>
    public string StatusDisplay => IsActive ? "Aktivní" : "Neaktivní";
    
    /// <summary>
    /// Status CSS class
    /// </summary>
    public string StatusClass => IsActive ? "text-success" : "text-danger";
}