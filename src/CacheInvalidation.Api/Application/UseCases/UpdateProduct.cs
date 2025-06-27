using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Domain.Events;
using CacheInvalidation.Api.Domain.Repositories;
using CacheInvalidation.Api.Notification;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class UpdateProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductUpdatedEvent> _notification;

        public UpdateProduct(IProductRepository repository, NotificationPublisher<ProductUpdatedEvent> notification)
        {
            _repository = repository;
            _notification = notification;
        }

        public async Task<Product> ExecuteAsync(Guid id, ProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            product.Name = string.IsNullOrWhiteSpace(dto.Name) ? product.Name : dto.Name;
            product.Description = string.IsNullOrWhiteSpace(dto.Description) ? product.Description : dto.Description;
            product.Price = dto.Price < 0 ? product.Price : new Money(dto.Price);
            product.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(id, product, cancellationToken);
            var @event = product.CreateProductUpdatedEvent();
            await _notification.ExecuteAsync(@event, cancellationToken);
            return product;

        }
    }
}
