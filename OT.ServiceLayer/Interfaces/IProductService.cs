using OT.ServiceLayer.DTOs;

namespace OT.ServiceLayer.Interfaces;

public interface IProductService : IBaseService<ProductDto>
{
    Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<ProductDto>> GetProductsInStockAsync();
}