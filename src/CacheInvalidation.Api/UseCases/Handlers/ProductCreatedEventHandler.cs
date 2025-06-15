using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.UseCases.Handlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig; 
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly RefreshProductCache _refreshProductCache;

        public ProductCreatedEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration, ILogger<ProductCreatedEventHandler> logger, RefreshProductCache refreshProductCache)
        {
            this._cacheDatabase = cacheDatabase;
            this._cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            this._logger = logger;
            this._refreshProductCache = refreshProductCache;
        }

        public async Task HandleAsync(ProductCreatedEvent notification, CancellationToken cancellationToken = default)
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
                nameof(ProductCreatedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            this._logger.LogInformation($"Product created event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
