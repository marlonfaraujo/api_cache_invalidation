using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.UseCases.Handlers
{
    public class ProductActivedEventHandler : INotificationHandler<ProductActivedEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;

        public ProductActivedEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration)
        {
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        }

        public Task HandleAsync(ProductActivedEvent notification, CancellationToken cancellationToken = default)
        {
            this._cacheDatabase.RemoverAsync(this._cacheConfig.ProductCacheKey, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
