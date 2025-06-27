using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Infra.Repositories;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class DisableProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductDisabledEvent> _notification;

        public DisableProduct(IProductRepository repository, NotificationPublisher<ProductDisabledEvent> notification)
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
            var @event = product.Disable();
            await _repository.UpdateAsync(id, product, cancellationToken);
            await _notification.ExecuteAsync(@event, cancellationToken);
        }
    }
}
