using CacheInvalidation.Api.Domain.Entities;

namespace CacheInvalidation.Api.Domain.Events
{
    public class ProductDisabledEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductDisabledEvent(Product product)
        {
            Product = product;
        }
    }
}
