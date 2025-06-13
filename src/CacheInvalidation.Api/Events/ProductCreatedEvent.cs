using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Events
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductCreatedEvent (Product product) 
        { 
            this.Product = product;
        }
    }
}
