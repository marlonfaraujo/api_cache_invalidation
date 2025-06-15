using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.UseCases.Handlers
{
    public class ProductDisabledEventHandler : INotificationHandler<ProductDisabledEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;
        private readonly ILogger<ProductDisabledEventHandler> _logger;
        private readonly RefreshProductCache _refreshProductCache;

        public ProductDisabledEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration, ILogger<ProductDisabledEventHandler> logger, RefreshProductCache refreshProductCache)
        {
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            this._logger = logger;
            this._refreshProductCache = refreshProductCache;
        }

        public async Task HandleAsync(ProductDisabledEvent notification, CancellationToken cancellationToken = default)
        {
            if (this._cacheConfig.ItsToRefresh.GetValueOrDefault() == true)
            {
                await this._refreshProductCache.ExecuteAsync(cancellationToken);
            }
            else
            {
                await this._cacheDatabase.RemoverAsync(this._cacheConfig.ProductCacheKey, cancellationToken);
            }
            var output = new OutboxMessage(notification.Product.Id,
                nameof(ProductDisabledEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            this._logger.LogInformation($"Product disabled event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
