using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Infra.Database;
using CacheInvalidation.Api.Infra.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class RefreshProductCache
    {
        private readonly IProductRepository _repository;
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;

        public RefreshProductCache(IProductRepository repository, ICacheDatabase cacheDatabase, IConfiguration configuration)
        {
            _repository = repository;
            _cacheDatabase = cacheDatabase;
            _cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var products = await _repository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                await _cacheDatabase.SetAsync(_cacheConfig.ProductCacheKey,
                    products, cancellationToken, TimeSpan.FromMinutes(_cacheConfig.ExpirationTimeMinutes));
            }
        }
    }
}
