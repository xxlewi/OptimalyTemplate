using System.Diagnostics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OT.DataLayer.Interfaces;
using OT.DataLayer.Entities;
using OT.ServiceLayer.DTOs;
using OT.ServiceLayer.Interfaces;

namespace OT.ServiceLayer.Services;

/// <summary>
/// Global search service implementation following CRM pattern
/// Provides unified search across multiple entity types with performance optimization
/// </summary>
public class SearchService : ISearchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Perform global search across all searchable entities
    /// </summary>
    public async Task<GlobalSearchResultDto> GlobalSearchAsync(string query, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        
        var stopwatch = Stopwatch.StartNew();
        var result = new GlobalSearchResultDto
        {
            Query = query,
            ResultCounts = new Dictionary<string, int>()
        };

        try
        {
            var searchTerm = query.Trim().ToLower();
            
            // Search products
            var productRepository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var products = await productRepository.Query
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(searchTerm) || 
                           (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                           (p.Sku != null && p.Sku.ToLower().Contains(searchTerm)))
                .Take(10) // Limit results for performance
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
                
            result.Products = _mapper.Map<IEnumerable<TemplateProductDto>>(products);
            result.ResultCounts["Products"] = products.Count;

            // Search categories
            var categoryRepository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var categories = await categoryRepository.Query
                .Where(c => c.Name.ToLower().Contains(searchTerm) || 
                           (c.Description != null && c.Description.ToLower().Contains(searchTerm)))
                .Take(10)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
                
            result.Categories = _mapper.Map<IEnumerable<TemplateCategoryDto>>(categories);
            result.ResultCounts["Categories"] = categories.Count;

            // Search users (if using custom User entity)
            var userRepository = _unitOfWork.GetRepository<User, string>();
            var users = await userRepository.Query
                .Where(u => u.FirstName.ToLower().Contains(searchTerm) ||
                           u.LastName.ToLower().Contains(searchTerm) ||
                           u.Email.ToLower().Contains(searchTerm))
                .Take(10)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
                
            result.Users = _mapper.Map<IEnumerable<UserDto>>(users);
            result.ResultCounts["Users"] = users.Count;

            // Calculate totals
            result.TotalResults = result.ResultCounts.Values.Sum();
            
            // Add search suggestions if no results
            if (result.TotalResults == 0)
            {
                result.Suggestions = await GetSearchSuggestionsAsync(searchTerm, 5, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            stopwatch.Stop();
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
        }

        return result;
    }

    /// <summary>
    /// Search within specific entity type
    /// </summary>
    public async Task<IEnumerable<TDto>> SearchAsync<TDto>(string query, CancellationToken cancellationToken = default) 
        where TDto : BaseDto
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        
        var searchTerm = query.Trim().ToLower();
        
        // This is a simplified implementation - in production, you'd want more sophisticated search
        if (typeof(TDto) == typeof(TemplateProductDto))
        {
            var repository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var entities = await repository.Query
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(searchTerm) || 
                           (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                           (p.Sku != null && p.Sku.ToLower().Contains(searchTerm)))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
                
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
        
        if (typeof(TDto) == typeof(TemplateCategoryDto))
        {
            var repository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var entities = await repository.Query
                .Where(c => c.Name.ToLower().Contains(searchTerm) || 
                           (c.Description != null && c.Description.ToLower().Contains(searchTerm)))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
                
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
        
        return Enumerable.Empty<TDto>();
    }

    /// <summary>
    /// Get search suggestions/auto-complete
    /// </summary>
    public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string partialQuery, int maxResults = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(partialQuery))
            return Enumerable.Empty<string>();
            
        var searchTerm = partialQuery.Trim().ToLower();
        var suggestions = new List<string>();

        try
        {
            // Get product name suggestions
            var productRepository = _unitOfWork.GetRepository<TemplateProduct, int>();
            var productSuggestions = await productRepository.Query
                .Where(p => p.Name.ToLower().StartsWith(searchTerm))
                .Select(p => p.Name)
                .Take(maxResults / 2)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            suggestions.AddRange(productSuggestions);

            // Get category name suggestions
            var categoryRepository = _unitOfWork.GetRepository<TemplateCategory, int>();
            var categorySuggestions = await categoryRepository.Query
                .Where(c => c.Name.ToLower().StartsWith(searchTerm))
                .Select(c => c.Name)
                .Take(maxResults / 2)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            suggestions.AddRange(categorySuggestions);

            return suggestions.Distinct().Take(maxResults);
        }
        catch
        {
            // Return empty suggestions on error
            return Enumerable.Empty<string>();
        }
    }
}