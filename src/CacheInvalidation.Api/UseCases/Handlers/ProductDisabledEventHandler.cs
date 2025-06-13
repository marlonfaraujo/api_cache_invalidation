using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.UseCases.Handlers
{
    public class ProductDisabledEventHandler : INotificationHandler<ProductDisabledEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;

        public ProductDisabledEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration)
        {
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        }

        public Task HandleAsync(ProductDisabledEvent notification, CancellationToken cancellationToken = default)
        {
            this._cacheDatabase.RemoverAsync(this._cacheConfig.ProductCacheKey, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
