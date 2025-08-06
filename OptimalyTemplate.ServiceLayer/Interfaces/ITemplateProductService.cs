using OptimalyTemplate.ServiceLayer.DTOs;
using OptimalyTemplate.ServiceLayer.Models;

namespace OptimalyTemplate.ServiceLayer.Interfaces;

/// <summary>
/// Service interface for TemplateProduct operations
/// Template service - remove in production
/// </summary>
public interface ITemplateProductService : IBaseService<TemplateProductDto>
{
    /// <summary>
    /// Get products by category
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get active products only
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get featured products
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> GetFeaturedProductsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get products on sale
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> GetProductsOnSaleAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get products with low stock (≤ 5)
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> GetLowStockProductsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get product by SKU
    /// </summary>
    Task<TemplateProductDto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Search products by name or description
    /// </summary>
    Task<IEnumerable<TemplateProductDto>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get products with filters and pagination
    /// </summary>
    Task<PagedResult<TemplateProductDto>> GetProductsPagedAsync(
        int page = 1, 
        int pageSize = 10,
        int? categoryId = null,
        bool? isActive = null,
        bool? isFeatured = null,
        string? searchTerm = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Update stock quantity
    /// </summary>
    Task UpdateStockAsync(int productId, int newQuantity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Bulk update active status
    /// </summary>
    Task BulkUpdateActiveStatusAsync(IEnumerable<int> productIds, bool isActive, CancellationToken cancellationToken = default);
}