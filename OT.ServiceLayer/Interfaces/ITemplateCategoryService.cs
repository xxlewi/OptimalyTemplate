using OT.ServiceLayer.DTOs;

namespace OT.ServiceLayer.Interfaces;

/// <summary>
/// Service interface for TemplateCategory operations
/// Template service - remove in production
/// </summary>
public interface ITemplateCategoryService : IBaseService<TemplateCategoryDto>
{
    /// <summary>
    /// Get all active categories ordered by DisplayOrder
    /// </summary>
    Task<IEnumerable<TemplateCategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get category by name
    /// </summary>
    Task<TemplateCategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get categories with product counts
    /// </summary>
    Task<IEnumerable<TemplateCategoryDto>> GetCategoriesWithProductCountsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reorder categories
    /// </summary>
    Task ReorderCategoriesAsync(Dictionary<int, int> categoryOrders, CancellationToken cancellationToken = default);
}