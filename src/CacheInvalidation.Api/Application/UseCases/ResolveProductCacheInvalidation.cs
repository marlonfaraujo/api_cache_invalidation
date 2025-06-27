using CacheInvalidation.Api.Application.Abstractions;
using CacheInvalidation.Api.Domain.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class ResolveProductCacheInvalidation
    {
        private readonly IProductRepository _repository;
        private readonly ICacheDatabase _cacheDatabase;
        private readonly ICacheConfig _cacheConfig;

        public ResolveProductCacheInvalidation(IProductRepository repository, ICacheDatabase cacheDatabase, ICacheConfig cacheConfig)
        {
            this._repository = repository;
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = cacheConfig;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (!this._cacheConfig.ItsToRefresh)
            {
                await this._cacheDatabase.RemoverAsync(this._cacheConfig.ProductCacheKey, cancellationToken);
                return;
            }
            var products = await this._repository.GetAsync(cancellationToken);
            if (products != null && products.Any())
            {
                await this._cacheDatabase.SetAsync(this._cacheConfig.ProductCacheKey,
                    products, cancellationToken, TimeSpan.FromMinutes(this._cacheConfig.ExpirationTimeMinutes));
            }
        }
    }
}
