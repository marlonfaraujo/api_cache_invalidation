using CacheInvalidation.Api.Application.Abstractions;
using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Domain.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class ListCachedProduct
    {
        private readonly IProductRepository _repository;
        private readonly ICacheDatabase _cacheDatabase;
        private readonly ICacheConfig _cacheConfig;

        public ListCachedProduct(IProductRepository repository, ICacheDatabase cacheDatabase, ICacheConfig cacheConfig)
        {
            _repository = repository;
            _cacheDatabase = cacheDatabase;
            _cacheConfig = cacheConfig;
        }

        public async Task<IEnumerable<ProductResultDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var cachedProducts = await _cacheDatabase.GetAsync<IEnumerable<Product>>(_cacheConfig.ProductCacheKey, cancellationToken);
            if (cachedProducts != null && cachedProducts.Any())
            {
                return GetProductsResult(cachedProducts);
            }
            var products = await _repository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                await _cacheDatabase.SetAsync(_cacheConfig.ProductCacheKey, 
                    products, cancellationToken, TimeSpan.FromMinutes(_cacheConfig.ExpirationTimeMinutes));
                return GetProductsResult(products);
            }
            return Enumerable.Empty<ProductResultDto>();
        }

        private IEnumerable<ProductResultDto> GetProductsResult(IEnumerable<Product> products) 
        {
            return products.Select(product => new ProductResultDto(
                product.Id.ToString(),
                product.Name,
                product.Description,
                product.Status,
                product.Price.Value,
                product.CreatedAt,
                product.UpdatedAt
                ));
        }
    }
}
