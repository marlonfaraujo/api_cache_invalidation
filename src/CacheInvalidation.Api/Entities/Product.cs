using CacheInvalidation.Api.Events;

namespace CacheInvalidation.Api.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Money Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;} 
        public DateTime? DeletedAt { get; set; } 

        public Product(string Name, string Description, decimal Price)
        {
            this.Id = Guid.NewGuid();
            this.Name = Name;
            this.Description = Description;
            this.Price = new Money(Price);
            this.Status = ProductStatusEnum.Actived.ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        public static Product Create(Guid id, string name, string description, string status, decimal price, DateTime createdAt, DateTime? updatedAt)
        {
            var product = new Product(name, description, price);
            product.Id = id;
            product.Status = status;
            product.CreatedAt = createdAt;
            product.UpdatedAt = updatedAt;

            return product;
        }

        public Product()
        {
            this.Id = Guid.NewGuid();
            this.Status = ProductStatusEnum.Actived.ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        public ProductCreatedEvent CreateProductCreatedEvent()
        {
            return new ProductCreatedEvent(this);
        }

        public ProductUpdatedEvent CreateProductUpdatedEvent()
        {
            return new ProductUpdatedEvent(this);
        }

        public ProductActivedEvent Active()
        {
            Status = ProductStatusEnum.Actived.ToString();
            UpdatedAt = DateTime.UtcNow;
            DeletedAt = null;
            return new ProductActivedEvent(this);
        }

        public ProductDisabledEvent Disable()
        {
            Status = ProductStatusEnum.Disabled.ToString();
            DeletedAt = DateTime.UtcNow;
            return new ProductDisabledEvent(this);
        }

    }
}
