using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class RefreshProductCache
    {
        private readonly IProductRepository _repository;
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;

        public RefreshProductCache(IProductRepository repository, ICacheDatabase cacheDatabase, IConfiguration configuration)
        {
            this._repository = repository;
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var products = await this._repository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                await this._cacheDatabase.SetAsync(this._cacheConfig.ProductCacheKey,
                    products, cancellationToken, TimeSpan.FromMinutes(this._cacheConfig.ExpirationTimeMinutes));
            }
        }
    }
}
