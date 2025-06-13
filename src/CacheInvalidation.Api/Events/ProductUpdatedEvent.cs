using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Events
{
    public class ProductUpdatedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductUpdatedEvent(Product product)
        {
            this.Product = product;
        }
    }
}
