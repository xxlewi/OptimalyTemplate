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
}