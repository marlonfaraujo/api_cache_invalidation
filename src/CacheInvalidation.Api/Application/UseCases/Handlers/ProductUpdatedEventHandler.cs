using CacheInvalidation.Api.Application.UseCases;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Infra.Database;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig;
        private readonly ILogger<ProductUpdatedEventHandler> _logger;
        private readonly RefreshProductCache _refreshProductCache;

        public ProductUpdatedEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration, ILogger<ProductUpdatedEventHandler> logger, RefreshProductCache refreshProductCache)
        {
            _cacheDatabase = cacheDatabase;
            _cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            _logger = logger;
            _refreshProductCache = refreshProductCache;
        }

        public async Task HandleAsync(ProductUpdatedEvent notification, CancellationToken cancellationToken = default)
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
                nameof(ProductUpdatedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            _logger.LogInformation($"Product updated event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
