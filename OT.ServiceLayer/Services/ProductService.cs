using AutoMapper;
using OT.DataLayer.Entities;
using OT.DataLayer.Interfaces;
using OT.ServiceLayer.DTOs;
using OT.ServiceLayer.Interfaces;

namespace OT.ServiceLayer.Services;

public class ProductService : BaseService<Product, ProductDto>, IProductService
{
    public ProductService(
        IRepository<Product> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : base(repository, unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = await _repository.FindAsync(p => p.Price >= minPrice && p.Price <= maxPrice);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsInStockAsync()
    {
        var products = await _repository.FindAsync(p => p.Stock > 0);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}