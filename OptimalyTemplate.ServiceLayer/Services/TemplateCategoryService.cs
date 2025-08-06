using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OptimalyTemplate.DataLayer.Entities;
using OptimalyTemplate.DataLayer.Interfaces;
using OptimalyTemplate.ServiceLayer.DTOs;
using OptimalyTemplate.ServiceLayer.Exceptions;
using OptimalyTemplate.ServiceLayer.Interfaces;

namespace OptimalyTemplate.ServiceLayer.Services;

/// <summary>
/// Service implementation for TemplateCategory operations
/// Template service - remove in production
/// </summary>
public class TemplateCategoryService : BaseService<TemplateCategory, TemplateCategoryDto, int>, ITemplateCategoryService
{
    public TemplateCategoryService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper)
    {
    }

    public override async Task<TemplateCategoryDto> CreateAsync(TemplateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // Business validation
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException(nameof(dto.Name), "Category name is required");

        // Check for duplicate name
        var existingCategory = await GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);
        if (existingCategory != null)
            throw new BusinessException($"Category with name '{dto.Name}' already exists", "CATEGORY_NAME_EXISTS");

        return await base.CreateAsync(dto, cancellationToken).ConfigureAwait(false);
    }

    public override async Task<TemplateCategoryDto> UpdateAsync(TemplateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // Business validation
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException(nameof(dto.Name), "Category name is required");

        // Check for duplicate name (excluding current category)
        var existingCategory = await GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);
        if (existingCategory != null && existingCategory.Id != dto.Id)
            throw new BusinessException($"Category with name '{dto.Name}' already exists", "CATEGORY_NAME_EXISTS");

        return await base.UpdateAsync(dto, cancellationToken).ConfigureAwait(false);
    }

    public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Check if category has products
        var repository = _unitOfWork.GetRepository<TemplateCategory, int>();
        var category = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        
        if (category == null)
            throw new NotFoundException(nameof(TemplateCategory), id);

        // Count products in category
        var productRepository = _unitOfWork.GetRepository<TemplateProduct, int>();
        var productsInCategory = await productRepository.FindAsync(p => p.CategoryId == id, cancellationToken).ConfigureAwait(false);
        
        if (productsInCategory.Any())
            throw new BusinessException($"Cannot delete category '{category.Name}' because it contains {productsInCategory.Count()} products", "CATEGORY_HAS_PRODUCTS");

        await base.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TemplateCategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var categories = await repository.FindAsync(c => c.IsActive, cancellationToken).ConfigureAwait(false);
            
            var sortedCategories = categories.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);
            return _mapper.Map<IEnumerable<TemplateCategoryDto>>(sortedCategories);
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve active categories", ex, "GET_ACTIVE_CATEGORIES_FAILED");
        }
    }

    public async Task<TemplateCategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Category name cannot be empty");

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var categories = await repository.FindAsync(c => c.Name == name.Trim(), cancellationToken).ConfigureAwait(false);
            var category = categories.FirstOrDefault();
            
            return category != null ? _mapper.Map<TemplateCategoryDto>(category) : null;
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException($"Failed to find category by name '{name}'", ex, "GET_CATEGORY_BY_NAME_FAILED");
        }
    }

    public async Task<IEnumerable<TemplateCategoryDto>> GetCategoriesWithProductCountsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var categoryRepository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var productRepository = _unitOfWork.GetRepository<TemplateProduct, int>();
            
            var categories = await categoryRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var products = await productRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            
            var categoryDtos = _mapper.Map<List<TemplateCategoryDto>>(categories.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name));
            
            // Set product counts
            foreach (var categoryDto in categoryDtos)
            {
                categoryDto.ProductCount = products.Count(p => p.CategoryId == categoryDto.Id);
            }
            
            return categoryDtos;
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to retrieve categories with product counts", ex, "GET_CATEGORIES_WITH_COUNTS_FAILED");
        }
    }

    public async Task ReorderCategoriesAsync(Dictionary<int, int> categoryOrders, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryOrders);

        if (!categoryOrders.Any())
            return;

        try
        {
            var repository = _unitOfWork.GetRepository<TemplateCategory, int>();
            
            foreach (var kvp in categoryOrders)
            {
                var category = await repository.GetByIdAsync(kvp.Key, cancellationToken).ConfigureAwait(false);
                if (category != null)
                {
                    category.DisplayOrder = kvp.Value;
                    repository.Update(category);
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (!(ex is BusinessException || ex is ValidationException || ex is NotFoundException))
        {
            throw new BusinessException("Failed to reorder categories", ex, "REORDER_CATEGORIES_FAILED");
        }
    }
}