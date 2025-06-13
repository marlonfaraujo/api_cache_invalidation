namespace CacheInvalidation.Api.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;} 
        public DateTime? DeletedAt { get; set; } 

        public Product(string Name, string Description, decimal Price)
        {
            this.Id = Guid.NewGuid();
            this.Name = Name;
            this.Description = Description;
            this.Price = Price;
            this.Status = ProductStatusEnum.Actived.ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        public Product(Guid id, string name, string description, string status, decimal price, DateTime createdAt, DateTime? updatedAt)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Status = status;
            this.Price = price;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
        }

        public Product()
        {
            this.Id = Guid.NewGuid();
            this.Status = ProductStatusEnum.Actived.ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        public void Active()
        {
            Status = ProductStatusEnum.Actived.ToString();
            UpdatedAt = DateTime.UtcNow;
        }

        public void Disable()
        {
            Status = ProductStatusEnum.Disabled.ToString();
            DeletedAt = DateTime.UtcNow;
        }

    }
}
