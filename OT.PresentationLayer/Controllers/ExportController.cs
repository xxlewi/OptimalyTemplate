using Microsoft.AspNetCore.Mvc;
using OT.ServiceLayer.Interfaces;
using OT.ServiceLayer.DTOs;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Controllers;

/// <summary>
/// Export controller following CRM pattern
/// Provides data export functionality in various formats
/// </summary>
public class ExportController : Controller
{
    private readonly IExportService _exportService;
    private readonly ITemplateProductService _productService;
    private readonly ITemplateCategoryService _categoryService;
    private readonly IUserService _userService;
    private readonly ILogger<ExportController> _logger;

    public ExportController(
        IExportService exportService,
        ITemplateProductService productService,
        ITemplateCategoryService categoryService,
        IUserService userService,
        ILogger<ExportController> logger)
    {
        _exportService = exportService;
        _productService = productService;
        _categoryService = categoryService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Export dashboard page
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new ExportViewModel();
        return View(viewModel);
    }

    /// <summary>
    /// Export products to specified format
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportProducts(ExportFormat format, bool activeOnly = false, int? categoryId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get products based on filters
            IEnumerable<TemplateProductDto> products;
            
            if (categoryId.HasValue)
            {
                products = await _productService.GetByCategoryAsync(categoryId.Value, cancellationToken);
            }
            else if (activeOnly)
            {
                products = await _productService.GetActiveProductsAsync(cancellationToken);
            }
            else
            {
                products = await _productService.GetAllAsync(cancellationToken);
            }

            // Generate export file
            byte[] fileData;
            string fileName;
            string contentType;

            switch (format)
            {
                case ExportFormat.Excel:
                    fileData = await _exportService.ExportToExcelAsync(products, "Products", cancellationToken);
                    fileName = $"products_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv"; // Basic CSV for now
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Csv:
                    fileData = await _exportService.ExportToCsvAsync(products, true, cancellationToken);
                    fileName = $"products_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Pdf:
                    fileData = await _exportService.ExportToPdfAsync(products, "Products Export", cancellationToken);
                    fileName = $"products_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt"; // Basic text for now
                    contentType = "text/plain";
                    break;
                
                case ExportFormat.Json:
                    fileData = await _exportService.ExportToJsonAsync(products, true, cancellationToken);
                    fileName = $"products_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
                    contentType = "application/json";
                    break;
                
                default:
                    throw new ArgumentException($"Unsupported export format: {format}");
            }

            _logger.LogInformation("Products exported: Format={Format}, Count={Count}", format, products.Count());

            return File(fileData, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting products in format {Format}", format);
            TempData["ErrorMessage"] = "Při exportu produktů došlo k chybě.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Export categories to specified format
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportCategories(ExportFormat format, bool activeOnly = false, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get categories
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            
            if (activeOnly)
            {
                categories = categories.Where(c => c.IsActive);
            }

            // Generate export file
            byte[] fileData;
            string fileName;
            string contentType;

            switch (format)
            {
                case ExportFormat.Excel:
                    fileData = await _exportService.ExportToExcelAsync(categories, "Categories", cancellationToken);
                    fileName = $"categories_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Csv:
                    fileData = await _exportService.ExportToCsvAsync(categories, true, cancellationToken);
                    fileName = $"categories_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Pdf:
                    fileData = await _exportService.ExportToPdfAsync(categories, "Categories Export", cancellationToken);
                    fileName = $"categories_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
                    contentType = "text/plain";
                    break;
                
                case ExportFormat.Json:
                    fileData = await _exportService.ExportToJsonAsync(categories, true, cancellationToken);
                    fileName = $"categories_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
                    contentType = "application/json";
                    break;
                
                default:
                    throw new ArgumentException($"Unsupported export format: {format}");
            }

            _logger.LogInformation("Categories exported: Format={Format}, Count={Count}", format, categories.Count());

            return File(fileData, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting categories in format {Format}", format);
            TempData["ErrorMessage"] = "Při exportu kategorií došlo k chybě.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Export users to specified format
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportUsers(ExportFormat format, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            // Generate export file
            byte[] fileData;
            string fileName;
            string contentType;

            switch (format)
            {
                case ExportFormat.Excel:
                    fileData = await _exportService.ExportToExcelAsync(users, "Users", cancellationToken);
                    fileName = $"users_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Csv:
                    fileData = await _exportService.ExportToCsvAsync(users, true, cancellationToken);
                    fileName = $"users_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                    contentType = "text/csv";
                    break;
                
                case ExportFormat.Pdf:
                    fileData = await _exportService.ExportToPdfAsync(users, "Users Export", cancellationToken);
                    fileName = $"users_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
                    contentType = "text/plain";
                    break;
                
                case ExportFormat.Json:
                    fileData = await _exportService.ExportToJsonAsync(users, true, cancellationToken);
                    fileName = $"users_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
                    contentType = "application/json";
                    break;
                
                default:
                    throw new ArgumentException($"Unsupported export format: {format}");
            }

            _logger.LogInformation("Users exported: Format={Format}, Count={Count}", format, users.Count());

            return File(fileData, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting users in format {Format}", format);
            TempData["ErrorMessage"] = "Při exportu uživatelů došlo k chybě.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Get export preview/statistics
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetExportStats(CancellationToken cancellationToken = default)
    {
        try
        {
            var allProducts = await _productService.GetAllAsync(cancellationToken);
            var activeProducts = await _productService.GetActiveProductsAsync(cancellationToken);
            var allCategories = await _categoryService.GetAllAsync(cancellationToken);
            var activeCategories = allCategories.Where(c => c.IsActive);
            var allUsers = await _userService.GetAllAsync(cancellationToken);

            var stats = new
            {
                products = new
                {
                    total = allProducts.Count(),
                    active = activeProducts.Count(),
                    inactive = allProducts.Count() - activeProducts.Count()
                },
                categories = new
                {
                    total = allCategories.Count(),
                    active = activeCategories.Count(),
                    inactive = allCategories.Count() - activeCategories.Count()
                },
                users = new
                {
                    total = allUsers.Count()
                }
            };

            return Json(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting export statistics");
            return Json(new { error = "Chyba při načítání statistik" });
        }
    }
}