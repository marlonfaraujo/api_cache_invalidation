using CacheInvalidation.Api.Application.UseCases;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Infra.Database;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ICacheDatabase _cacheDatabase;
        private readonly CacheConfig _cacheConfig; 
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly RefreshProductCache _refreshProductCache;

        public ProductCreatedEventHandler(ICacheDatabase cacheDatabase, IConfiguration configuration, ILogger<ProductCreatedEventHandler> logger, RefreshProductCache refreshProductCache)
        {
            _cacheDatabase = cacheDatabase;
            _cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            _logger = logger;
            _refreshProductCache = refreshProductCache;
        }

        public async Task HandleAsync(ProductCreatedEvent notification, CancellationToken cancellationToken = default)
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
                nameof(ProductCreatedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            _logger.LogInformation($"Product created event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
