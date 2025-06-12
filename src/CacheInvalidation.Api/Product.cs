namespace CacheInvalidation.Api
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
            this.Status = ProductStatusEnum.ACTIVED.ToString();
            this.CreatedAt = DateTime.Now;
        }

        public void Active()
        {
            this.Status = ProductStatusEnum.ACTIVED.ToString();
            this.UpdatedAt = DateTime.Now;
        }

        public void Disable()
        {
            this.Status = ProductStatusEnum.DISABLED.ToString();
            this.DeletedAt = DateTime.Now;
        }

    }
}
