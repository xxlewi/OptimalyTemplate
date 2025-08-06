using System.ComponentModel.DataAnnotations;

namespace OptimalyTemplate.PresentationLayer.ViewModels;

/// <summary>
/// ViewModel for TemplateProduct CRUD operations
/// Template ViewModel - remove in production
/// </summary>
public class TemplateProductViewModel : BaseViewModel
{
    /// <summary>
    /// Product name
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Product description
    /// </summary>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Product price
    /// </summary>
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Sale price (optional)
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Sale price must be between 0.01 and 999,999.99")]
    [DataType(DataType.Currency)]
    [Display(Name = "Sale Price")]
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// Stock quantity
    /// </summary>
    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }
    
    /// <summary>
    /// Product SKU
    /// </summary>
    [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
    [Display(Name = "SKU")]
    public string? Sku { get; set; }
    
    /// <summary>
    /// Is product active
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Is product featured
    /// </summary>
    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }
    
    /// <summary>
    /// Category ID
    /// </summary>
    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Category name (for display)
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Available categories for dropdown
    /// </summary>
    public IEnumerable<TemplateCategoryViewModel> AvailableCategories { get; set; } = new List<TemplateCategoryViewModel>();
    
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
        0 => "Out of Stock",
        <= 5 => "Low Stock",
        _ => "In Stock"
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

/// <summary>
/// ViewModel for TemplateCategory
/// Template ViewModel - remove in production
/// </summary>
public class TemplateCategoryViewModel : BaseViewModel
{
    /// <summary>
    /// Category name
    /// </summary>
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Category description
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Display order
    /// </summary>
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Is category active
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Number of products in category
    /// </summary>
    public int ProductCount { get; set; }
    
    /// <summary>
    /// Display name with product count
    /// </summary>
    public string DisplayName => $"{Name} ({ProductCount} products)";
}

/// <summary>
/// ViewModel for pagination info
/// </summary>
public class PaginationViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }  
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
    public int StartItem => TotalItems == 0 ? 0 : (Page - 1) * PageSize + 1;
    public int EndItem => Math.Min(StartItem + PageSize - 1, TotalItems);
}

/// <summary>
/// ViewModel for product search and filtering
/// </summary>
public class ProductSearchViewModel
{
    /// <summary>
    /// Search term
    /// </summary>
    [Display(Name = "Search")]
    public string? SearchTerm { get; set; }
    
    /// <summary>
    /// Category filter
    /// </summary>
    [Display(Name = "Category")]
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// Active status filter
    /// </summary>
    [Display(Name = "Status")]
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Featured filter
    /// </summary>
    [Display(Name = "Featured Only")]
    public bool? IsFeatured { get; set; }
    
    /// <summary>
    /// Sort field
    /// </summary>
    [Display(Name = "Sort By")]
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Sort direction
    /// </summary>
    [Display(Name = "Sort Order")]
    public bool SortDescending { get; set; }
    
    /// <summary>
    /// Page number
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Available categories for filter
    /// </summary>
    public IEnumerable<TemplateCategoryViewModel> AvailableCategories { get; set; } = new List<TemplateCategoryViewModel>();
}