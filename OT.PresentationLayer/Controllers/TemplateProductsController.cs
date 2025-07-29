using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OT.ServiceLayer.Interfaces;
using OT.ServiceLayer.Exceptions;
using OT.ServiceLayer.DTOs;
using OT.ServiceLayer.Models;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Controllers;

/// <summary>
/// Controller for TemplateProduct CRUD operations
/// Template controller - remove in production
/// </summary>
[Authorize]
public class TemplateProductsController : Controller
{
    private readonly ITemplateProductService _productService;
    private readonly ITemplateCategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<TemplateProductsController> _logger;

    public TemplateProductsController(
        ITemplateProductService productService,
        ITemplateCategoryService categoryService,
        IMapper mapper,
        ILogger<TemplateProductsController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: TemplateProducts
    public async Task<IActionResult> Index(
        string? searchTerm,
        int? categoryId,
        bool? isActive,
        bool? isFeatured,
        string? sortBy,
        bool sortDescending = false,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            // Get categories for filter dropdown
            var categories = await _categoryService.GetActiveCategoriesAsync();
            var categoryViewModels = _mapper.Map<List<TemplateCategoryViewModel>>(categories);

            // Get paged products
            var pagedResult = await _productService.GetProductsPagedAsync(
                page, pageSize, categoryId, isActive, isFeatured, 
                searchTerm, sortBy, sortDescending);

            var productViewModels = pagedResult.Items.Select(p => new TemplateProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SalePrice = p.SalePrice,
                StockQuantity = p.StockQuantity,
                Sku = p.Sku,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            // Create search view model
            var searchViewModel = new ProductSearchViewModel
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                IsActive = isActive,
                IsFeatured = isFeatured,
                SortBy = sortBy,
                SortDescending = sortDescending,
                Page = page,
                PageSize = pageSize,
                AvailableCategories = categoryViewModels
            };

            ViewBag.SearchModel = searchViewModel;
            ViewBag.PagedResult = pagedResult;
            ViewBag.Categories = categoryViewModels;

            return View(productViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products for index page");
            TempData["ErrorMessage"] = "Error loading products. Please try again.";
            return View(new List<TemplateProductViewModel>());
        }
    }

    // GET: TemplateProducts/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<TemplateProductViewModel>(product);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product details for ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error loading product details. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: TemplateProducts/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            var viewModel = new TemplateProductViewModel();
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparing create product page");
            TempData["ErrorMessage"] = "Error loading create form. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: TemplateProducts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TemplateProductViewModel viewModel)
    {
        try
        {
            // Custom validation
            if (viewModel.SalePrice.HasValue && viewModel.SalePrice >= viewModel.Price)
            {
                ModelState.AddModelError(nameof(viewModel.SalePrice), "Sale price must be less than regular price.");
            }

            if (ModelState.IsValid)
            {
                var productDto = _mapper.Map<ServiceLayer.DTOs.TemplateProductDto>(viewModel);
                var createdProduct = await _productService.CreateAsync(productDto);

                TempData["SuccessMessage"] = $"Product '{createdProduct.Name}' created successfully.";
                return RedirectToAction(nameof(Details), new { id = createdProduct.Id });
            }

            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                foreach (var message in error.Value)
                {
                    ModelState.AddModelError(error.Key, message);
                }
            }
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            TempData["ErrorMessage"] = "Error creating product. Please try again.";
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
    }

    // GET: TemplateProducts/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<TemplateProductViewModel>(product);
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparing edit product page for ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error loading edit form. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: TemplateProducts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TemplateProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ErrorMessage"] = "Invalid product ID.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Custom validation
            if (viewModel.SalePrice.HasValue && viewModel.SalePrice >= viewModel.Price)
            {
                ModelState.AddModelError(nameof(viewModel.SalePrice), "Sale price must be less than regular price.");
            }

            if (ModelState.IsValid)
            {
                var productDto = _mapper.Map<ServiceLayer.DTOs.TemplateProductDto>(viewModel);
                var updatedProduct = await _productService.UpdateAsync(productDto);

                TempData["SuccessMessage"] = $"Product '{updatedProduct.Name}' updated successfully.";
                return RedirectToAction(nameof(Details), new { id = updatedProduct.Id });
            }

            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                foreach (var message in error.Value)
                {
                    ModelState.AddModelError(error.Key, message);
                }
            }
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error updating product. Please try again.";
            await LoadCategoriesAsync(viewModel);
            return View(viewModel);
        }
    }

    // GET: TemplateProducts/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<TemplateProductViewModel>(product);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparing delete product page for ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error loading delete confirmation. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: TemplateProducts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            await _productService.DeleteAsync(id);
            TempData["SuccessMessage"] = $"Product '{product.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error deleting product. Please try again.";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    // POST: TemplateProducts/UpdateStock
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStock(int id, int newQuantity)
    {
        try
        {
            await _productService.UpdateStockAsync(id, newQuantity);
            TempData["SuccessMessage"] = "Stock quantity updated successfully.";
        }
        catch (ValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating stock for product ID {ProductId}", id);
            TempData["ErrorMessage"] = "Error updating stock. Please try again.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // API endpoint for DataTables
    [HttpPost]
    public async Task<IActionResult> GetProductsJson(
        int draw,
        int start,
        int length,
        string? searchValue,
        int? categoryFilter)
    {
        try
        {
            var page = (start / length) + 1;
            var pagedResult = await _productService.GetProductsPagedAsync(
                page, length, categoryFilter, searchTerm: searchValue);

            var data = pagedResult.Items.Select(p => new
            {
                p.Id,
                p.Name,
                p.CategoryName,
                FormattedPrice = p.FormattedPrice,
                FormattedSalePrice = p.FormattedSalePrice,
                p.StockQuantity,
                StockStatus = p.StockStatus,
                StockStatusClass = p.StockStatusClass,
                IsActive = p.IsActive ? "Yes" : "No",
                IsFeatured = p.IsFeatured ? "Yes" : "No",
                CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd")
            });

            return Json(new
            {
                draw,
                recordsTotal = pagedResult.TotalItems,
                recordsFiltered = pagedResult.TotalItems,
                data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products for DataTables");
            return Json(new { error = "Error loading data" });
        }
    }

    private async Task LoadCategoriesAsync(TemplateProductViewModel viewModel)
    {
        var categories = await _categoryService.GetActiveCategoriesAsync();
        viewModel.AvailableCategories = _mapper.Map<List<TemplateCategoryViewModel>>(categories);
    }
}