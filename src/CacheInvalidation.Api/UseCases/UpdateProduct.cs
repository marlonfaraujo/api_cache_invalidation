using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Notification;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class UpdateProduct
    {
        private readonly IProductRepository _repository;
        private readonly NotificationPublisher<ProductUpdatedEvent> _notification;

        public UpdateProduct(IProductRepository repository, NotificationPublisher<ProductUpdatedEvent> notification)
        {
            this._repository = repository;
            this._notification = notification;
        }

        public async Task<Product> ExecuteAsync(Guid id, ProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = await this._repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            product.Name = string.IsNullOrWhiteSpace(dto.Name) ? product.Name : dto.Name;
            product.Description = string.IsNullOrWhiteSpace(dto.Description) ? product.Description : dto.Description;
            product.Price = dto.Price < 0 ? product.Price : new Money(dto.Price);
            product.UpdatedAt = DateTime.UtcNow;
            await this._repository.UpdateAsync(id, product, cancellationToken);
            var @event = product.CreateProductUpdatedEvent();
            await this._notification.ExecuteAsync(@event, cancellationToken);
            return product;

        }
    }
}
