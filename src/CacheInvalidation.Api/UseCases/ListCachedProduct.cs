using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class ListCachedProduct
    {
        private readonly IProductRepository _repository;
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;

        public ListCachedProduct(IProductRepository repository, ICacheDatabase cacheDatabase, IConfiguration configuration)
        {
            this._repository = repository;
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        }

        public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var cachedProducts = await this._cacheDatabase.GetAsync<IEnumerable<Product>>(this._cacheConfig.ProductCacheKey, cancellationToken);
            if (cachedProducts != null && cachedProducts.Any())
            {
                return cachedProducts;
            }

            var products = await this._repository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                await this._cacheDatabase.SetAsync(this._cacheConfig.ProductCacheKey, 
                    products, cancellationToken, TimeSpan.FromMinutes(this._cacheConfig.ExpirationTimeMinutes));
                return products;
            }
            return Enumerable.Empty<Product>();
        }
    }
}
