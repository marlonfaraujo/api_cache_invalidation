using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
    {
        private readonly ILogger<ProductUpdatedEventHandler> _logger;
        private readonly ResolveProductCacheInvalidation _resolveProductCacheInvalidation;

        public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger, ResolveProductCacheInvalidation resolveProductCacheInvalidation)
        {
            this._logger = logger;
            this._resolveProductCacheInvalidation = resolveProductCacheInvalidation;
        }

        public async Task HandleAsync(ProductUpdatedEvent notification, CancellationToken cancellationToken = default)
        {
            await this._resolveProductCacheInvalidation.ExecuteAsync(cancellationToken);
            var output = new OutputMessage(notification.Product.Id,
                nameof(ProductUpdatedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            this._logger.LogInformation($"Product updated event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
