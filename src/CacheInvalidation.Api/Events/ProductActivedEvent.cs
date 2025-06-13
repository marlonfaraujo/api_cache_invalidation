using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Events
{
    public class ProductActivedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductActivedEvent(Product product)
        {
            this.Product = product;
        }
    }
}
