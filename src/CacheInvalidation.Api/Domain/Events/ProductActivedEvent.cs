using CacheInvalidation.Api.Domain.Entities;

namespace CacheInvalidation.Api.Domain.Events
{
    public class ProductActivedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductActivedEvent(Product product)
        {
            Product = product;
        }
    }
}
