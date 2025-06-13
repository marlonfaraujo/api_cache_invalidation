using Bogus;
using CacheInvalidation.Api.Dtos;

namespace CacheInvalidation.Integration
{
    public class ProductDtoTestData
    {
        private static readonly Faker<ProductDto> ProductDtoFaker = new Faker<ProductDto>()
            .CustomInstantiator(x => new ProductDto(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 1000)));

        public static ProductDto GenerateProduct()
        {
            return ProductDtoFaker.Generate();
        }
    }
}
