using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class CreateProduct
    {
        private readonly IProductRepository _repository; 
        private readonly NotificationPublisher<ProductCreatedEvent> _notification;

        public CreateProduct(IProductRepository repository, NotificationPublisher<ProductCreatedEvent> notification)
        {
            this._repository = repository;
            this._notification = notification;
        }

        public async Task<Product> ExecuteAsync(ProductDto dto, CancellationToken cancellationToken = default) 
        {
            var product = new Product(dto.Name, dto.Description, dto.Price);
            await this._repository.CreateAsync(product, cancellationToken);
            var @event = product.CreateProductCreatedEvent();
            await this._notification.ExecuteAsync(@event, cancellationToken);
            return product;
        }
    }
}
