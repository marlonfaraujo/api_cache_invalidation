using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Domain.Repositories;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class ActiveProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductActivedEvent> _notification;

        public ActiveProduct(IProductRepository repository, NotificationPublisher<ProductActivedEvent> notification)
        {
            _repository = repository;
            _notification = notification;
        }

        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            var @event = product.Active();
            await _repository.UpdateAsync(id, product, cancellationToken);
            await _notification.ExecuteAsync(@event, cancellationToken);
        }
    }
}
