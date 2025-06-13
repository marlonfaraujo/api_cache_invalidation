using Bogus;
using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Integration
{
    public class ProductTestData
    {
        private static readonly Faker<Product> ProductFaker = new Faker<Product>()
            .RuleFor(u => u.Name, f => f.Commerce.ProductName())
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.Price, f => new Money(f.Random.Decimal(1,1000)));

        public static Product GenerateProduct()
        {
            return ProductFaker.Generate();
        }
    }
}
