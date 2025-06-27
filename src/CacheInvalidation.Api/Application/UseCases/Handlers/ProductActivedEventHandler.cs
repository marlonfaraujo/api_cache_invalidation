using CacheInvalidation.Api.Application.UseCases;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Infra.Database;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductActivedEventHandler : INotificationHandler<ProductActivedEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;
        private readonly ILogger<ProductActivedEventHandler> _logger;
        private readonly RefreshProductCache _refreshProductCache;

        public ProductActivedEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration, ILogger<ProductActivedEventHandler> logger, RefreshProductCache refreshProductCache)
        {
            _cacheDatabase = cacheDatabase;
            _cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            _logger = logger;
            _refreshProductCache = refreshProductCache;
        }

        public async Task HandleAsync(ProductActivedEvent notification, CancellationToken cancellationToken = default)
        {
            if (_cacheConfig.ItsToRefresh.GetValueOrDefault() == true)
            {
                await _refreshProductCache.ExecuteAsync(cancellationToken);
            }
            else
            {
                await _cacheDatabase.RemoverAsync(_cacheConfig.ProductCacheKey, cancellationToken);
            }
            var output = new OutboxMessage(notification.Product.Id,
                nameof(ProductActivedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            _logger.LogInformation($"Product actived event handled: {JsonSerializer.Serialize(output)}");
            
        }
    }
}
