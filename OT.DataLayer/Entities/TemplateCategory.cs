using OT.DataLayer.Interfaces;

namespace OT.DataLayer.Entities;

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
    /// Computed property for display
    /// </summary>
    public string DisplayName => $"{Name} ({Products.Count} produktů)";
}