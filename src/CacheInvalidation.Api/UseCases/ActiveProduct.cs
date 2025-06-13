using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class ActiveProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductActivedEvent> _notification;

        public ActiveProduct(IProductRepository repository, NotificationPublisher<ProductActivedEvent> notification)
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
            var @event = product.Active();
            await this._repository.UpdateAsync(id, product, cancellationToken);
            await this._notification.ExecuteAsync(@event, cancellationToken);
        }
    }
}
