using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class DisableProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductDisabledEvent> _notification;

        public DisableProduct(IProductRepository repository, NotificationPublisher<ProductDisabledEvent> notification)
        {
            this._repository = repository;
            this._notification = notification;
        }

        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await this._repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            var @event = product.Disable();
            await this._repository.UpdateAsync(id, product, cancellationToken);
            await this._notification.ExecuteAsync(@event, cancellationToken);
        }
    }
}
