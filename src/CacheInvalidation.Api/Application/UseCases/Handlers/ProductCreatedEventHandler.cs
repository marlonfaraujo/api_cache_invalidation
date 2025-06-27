using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Notification;
using System.Text.Json;

namespace CacheInvalidation.Api.Application.UseCases.Handlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly ResolveProductCacheInvalidation _resolveProductCacheInvalidation;

        public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, ResolveProductCacheInvalidation resolveProductCacheInvalidation)
        {
            this._logger = logger;
            this._resolveProductCacheInvalidation = resolveProductCacheInvalidation;
        }

        public async Task HandleAsync(ProductCreatedEvent notification, CancellationToken cancellationToken = default)
        {
            await this._resolveProductCacheInvalidation.ExecuteAsync(cancellationToken);
            var output = new OutputMessage(notification.Product.Id, 
                nameof(ProductCreatedEvent).ToString(),
                JsonSerializer.Serialize(notification),
                DateTime.UtcNow,
                false);
            this._logger.LogInformation($"Product created event handled: {JsonSerializer.Serialize(output)}");
        }
    }
}
