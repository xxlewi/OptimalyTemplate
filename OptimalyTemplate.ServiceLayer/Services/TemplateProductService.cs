using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OptimalyTemplate.DataLayer.Entities;
using OptimalyTemplate.DataLayer.Interfaces;
using OptimalyTemplate.ServiceLayer.DTOs;
using OptimalyTemplate.ServiceLayer.Exceptions;
using OptimalyTemplate.ServiceLayer.Interfaces;
using OptimalyTemplate.ServiceLayer.Models;

namespace OptimalyTemplate.ServiceLayer.Services;

/// <summary>
/// Service implementation for TemplateProduct operations
/// Template service - remove in production
/// </summary>
public class TemplateProductService : BaseService<TemplateProduct, TemplateProductDto, int>, ITemplateProductService
{
    public TemplateProductService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper)
    {
    }

    public override async Task<TemplateProductDto> CreateAsync(TemplateProductDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // Business validation
        await ValidateProductAsync(dto, isUpdate: false, cancellationToken).ConfigureAwait(false);

        return await base.CreateAsync(dto, cancellationToken).ConfigureAwait(false);
    }

    public override async Task<TemplateProductDto> UpdateAsync(TemplateProductDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // Business validation
        await ValidateProductAsync(dto, isUpdate: true, cancellationToken).ConfigureAwait(false);

        return await base.UpdateAsync(dto, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TemplateProductDto>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.CategoryId == categoryId, cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException($"Failed to retrieve products for category {categoryId}", ex, "GET_PRODUCTS_BY_CATEGORY_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateProductDto>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.IsActive, cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve active products", ex, "GET_ACTIVE_PRODUCTS_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateProductDto>> GetFeaturedProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.IsFeatured && p.IsActive, cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve featured products", ex, "GET_FEATURED_PRODUCTS_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateProductDto>> GetProductsOnSaleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.SalePrice.HasValue && p.SalePrice < p.Price && p.IsActive, cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve products on sale", ex, "GET_SALE_PRODUCTS_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateProductDto>> GetLowStockProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.StockQuantity <= 5 && p.IsActive, cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.StockQuantity).ThenBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve low stock products", ex, "GET_LOW_STOCK_PRODUCTS_FAILED");
        }
    }

    public async Task<TemplateProductDto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ValidationException(nameof(sku), "SKU cannot be empty");

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await repository.FindAsync(p => p.Sku == sku.Trim(), cancellationToken).ConfigureAwait(false);
            var product = products.FirstOrDefault();
            
            return product != null ? _mapper.Map<TemplateProductDto>(product) : null;
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException($"Failed to find product by SKU '{sku}'", ex, "GET_PRODUCT_BY_SKU_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateProductDto>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<TemplateProductDto>();

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var term = searchTerm.Trim().ToLower();
            
            var products = await repository.FindAsync(p => 
                p.Name.ToLower().Contains(term) || 
                (p.Description != null && p.Description.ToLower().Contains(term)) ||
                (p.Sku != null && p.Sku.ToLower().Contains(term)), 
                cancellationToken).ConfigureAwait(false);
            
            return _mapper.Map<IEnumerable<TemplateProductDto>>(products.OrderBy(p => p.Name));
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException($"Failed to search products with term '{searchTerm}'", ex, "SEARCH_PRODUCTS_FAILED");
        }
    }


    public async Task UpdateStockAsync(int productId, int newQuantity, CancellationToken cancellationToken = default)
    {
        if (newQuantity < 0)
            throw TemplateValidationException.ForProduct(productId, nameof(newQuantity), "Skladové množství nemůže být záporné");

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var product = await repository.GetByIdAsync(productId, cancellationToken).ConfigureAwait(false);
            
            if (product == null)
                throw new NotFoundException(nameof(TemplateProduct), productId);

            product.StockQuantity = newQuantity;
            repository.Update(product);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException($"Failed to update stock for product {productId}", ex, "UPDATE_STOCK_FAILED");
        }
    }

    public async Task BulkUpdateActiveStatusAsync(IEnumerable<int> productIds, bool isActive, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productIds);
        
        var ids = productIds.ToList();
        if (!ids.Any())
            return;

        // Business rule: limit bulk operations to prevent performance issues
        const int maxBulkSize = 100;
        if (ids.Count > maxBulkSize)
            throw TemplateBusinessException.BulkOperationLimit("Bulk Update Active Status", ids.Count, maxBulkSize);

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            
            foreach (var id in ids)
            {
                var product = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
                if (product != null)
                {
                    product.IsActive = isActive;
                    repository.Update(product);
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to bulk update product active status", ex, "BULK_UPDATE_ACTIVE_STATUS_FAILED");
        }
    }

    private async Task ValidateProductAsync(TemplateProductDto dto, bool isUpdate, CancellationToken cancellationToken)
    {
        // Required field validation using template-specific exceptions
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw TemplateValidationException.ForProduct(nameof(dto.Name), "Název produktu je povinný");

        if (dto.Price <= 0)
            throw TemplateValidationException.ForProduct(nameof(dto.Price), "Cena produktu musí být větší než nula");

        if (dto.SalePrice.HasValue && dto.SalePrice <= 0)
            throw TemplateValidationException.ForProduct(nameof(dto.SalePrice), "Akční cena musí být větší než nula");

        // Business rule validation using template-specific exceptions
        if (dto.SalePrice.HasValue && dto.SalePrice >= dto.Price)
        {
            if (isUpdate && dto.Id > 0)
                throw TemplateBusinessException.InvalidSalePrice(dto.Id, dto.Price, dto.SalePrice.Value);
            else
                throw TemplateValidationException.ForProduct(nameof(dto.SalePrice), "Akční cena musí být nižší než běžná cena");
        }

        if (dto.StockQuantity < 0)
            throw TemplateValidationException.ForProduct(nameof(dto.StockQuantity), "Skladové množství nemůže být záporné");

        // Category validation with business rules
        var categoryRepository = _unitOfWork.GetRepository<TemplateCategory, int>();
        var category = await categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken).ConfigureAwait(false);
        if (category == null)
            throw TemplateValidationException.ForProduct(nameof(dto.CategoryId), "Vybraná kategorie neexistuje");

        if (!category.IsActive)
            throw TemplateBusinessException.InactiveCategoryAssignment(dto.CategoryId, category.Name);

        // Product activation validation
        if (dto.IsActive && dto.CategoryId == 0)
            throw TemplateBusinessException.ProductActivationWithoutCategory(dto.Id);

        // SKU uniqueness validation with business exception
        if (!string.IsNullOrWhiteSpace(dto.Sku))
        {
            var existingProduct = await GetBySkuAsync(dto.Sku, cancellationToken).ConfigureAwait(false);
            if (existingProduct != null && (!isUpdate || existingProduct.Id != dto.Id))
                throw TemplateBusinessException.DuplicateSku(dto.Sku, existingProduct.Id);
        }
    }

    public async Task<PagedResult<TemplateProductDto>> GetProductsPagedAsync(
        int page = 1, 
        int pageSize = 10,
        int? categoryId = null,
        bool? isActive = null,
        bool? isFeatured = null,
        string? searchTerm = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Build query with proper eager loading
            IQueryable<TemplateProduct> query = _repository.Query.Include(p => p.Category);
            
            // Apply filters
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);
                
            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);
                
            if (isFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == isFeatured.Value);
                
            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm) || 
                                        (p.Description != null && p.Description.Contains(searchTerm)) ||
                                        (p.Sku != null && p.Sku.Contains(searchTerm)));
            
            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "price" => sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "stock" => sortDescending ? query.OrderByDescending(p => p.StockQuantity) : query.OrderBy(p => p.StockQuantity),
                    "category" => sortDescending ? query.OrderByDescending(p => p.Category.Name) : query.OrderBy(p => p.Category.Name),
                    "created" => sortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderBy(p => p.Name)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }
            
            // Get total count
            var totalItems = await query.CountAsync(cancellationToken);
            
            // Apply pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            
            var dtos = _mapper.Map<List<TemplateProductDto>>(items);
            
            return new PagedResult<TemplateProductDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve paged products", ex, "GET_PAGED_PRODUCTS_FAILED");
        }
    }
}