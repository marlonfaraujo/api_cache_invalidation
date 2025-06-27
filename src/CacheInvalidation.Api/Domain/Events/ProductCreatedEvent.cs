using CacheInvalidation.Api.Domain.Entities;

namespace CacheInvalidation.Api.Domain.Events
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductCreatedEvent (Product product) 
        {
            Product = product;
        }
    }
}
