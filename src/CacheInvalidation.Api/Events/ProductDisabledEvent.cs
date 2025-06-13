using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Events
{
    public class ProductDisabledEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductDisabledEvent(Product product)
        {
            this.Product = product;
        }
    }
}
