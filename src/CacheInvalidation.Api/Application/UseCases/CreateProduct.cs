using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Domain.Repositories;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class CreateProduct
    {
        private readonly IProductRepository _repository; 
        private readonly NotificationPublisher<ProductCreatedEvent> _notification;

        public CreateProduct(IProductRepository repository, NotificationPublisher<ProductCreatedEvent> notification)
        {
            _repository = repository;
            _notification = notification;
        }

        public async Task<Product> ExecuteAsync(ProductDto dto, CancellationToken cancellationToken = default) 
        {
            var product = new Product(dto.Name, dto.Description, dto.Price);
            await _repository.CreateAsync(product, cancellationToken);
            var @event = product.CreateProductCreatedEvent();
            await _notification.ExecuteAsync(@event, cancellationToken);
            return product;
        }
    }
}
