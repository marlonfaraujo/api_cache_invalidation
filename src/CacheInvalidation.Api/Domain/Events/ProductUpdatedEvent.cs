using CacheInvalidation.Api.Domain.Entities;

namespace CacheInvalidation.Api.Domain.Events
{
    public class ProductUpdatedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductUpdatedEvent(Product product)
        {
            Product = product;
        }
    }
}
