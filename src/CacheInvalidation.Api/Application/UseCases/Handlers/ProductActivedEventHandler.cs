using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductActivedEventHandler : INotificationHandler<ProductActivedEvent>
    {
        private readonly ILogger<ProductActivedEventHandler> _logger;
        private readonly ResolveProductCacheInvalidation _resolveProductCacheInvalidation;

        public ProductActivedEventHandler(ILogger<ProductActivedEventHandler> logger, ResolveProductCacheInvalidation resolveProductCacheInvalidation)
        {
            this._logger = logger;
            this._resolveProductCacheInvalidation = resolveProductCacheInvalidation;
        }

        public async Task HandleAsync(ProductActivedEvent notification, CancellationToken cancellationToken = default)
        {
            await this._resolveProductCacheInvalidation.ExecuteAsync(cancellationToken);
            var output = new OutputMessage(notification.Product.Id,
                nameof(ProductActivedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            this._logger.LogInformation($"Product actived event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
