using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Repositories;
using Microsoft.Extensions.Configuration;

namespace CacheInvalidation.Integration.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly IProductRepository _repository;
        private readonly IDatabaseConnection _postgresDb;

        public ProductRepositoryTest()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", "Host=localhost;Port=5432;Database=developer;Username=postgres;Password=postgres"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            this._postgresDb = new PostgresDatabaseConnection(configuration);
            this._repository = new ProductDatabaseRepository(this._postgresDb);
        }

        [Fact(DisplayName = "Given a fake product object when saving to the database then it should return product registration")]
        public async Task Given_FakeProductObject_When_SavingToTheDatabase_Then_ItShouldReturnProductRegistration()
        {
            var product = ProductTestData.GenerateProduct();
            await this._repository.CreateAsync(product);
            var productData = await this._repository.GetByIdAsync(product.Id);
            Assert.NotNull(productData);
        }

        [Fact(DisplayName = "Given several fake product objects when saving to the database then a list of registered products should be returned")]
        public async Task Given_SeveralFakeProductObjects_When_SavingToTheDatabase_Then_ListOfRegisteredProductsShouldBeReturned()
        {
            await this._postgresDb.ExecuteAsync("delete from products", CancellationToken.None, new {});
            int length = 5;
            for(int i = 0; i < length; i++)
            {
                var product = ProductTestData.GenerateProduct();
                await this._repository.CreateAsync(product);
            }  
            var productData = await this._repository.GetAsync();
            Assert.NotNull(productData);
            Assert.NotEmpty(productData);
            Assert.Equal(length, productData.Count());
        }

        [Fact(DisplayName = "Given a product registered in the database when updating its fields then an updated product should be returned")]
        public async Task Given_ProductRegisteredInTheDatabase_When_UpdatingItsFields_Then_UpdatedProductShouldBeReturned()
        {
            var product = ProductTestData.GenerateProduct();
            await this._repository.CreateAsync(product);
            var productData = await this._repository.GetByIdAsync(product.Id);
            Assert.NotNull(productData);

            var toUpdateProduct = ProductTestData.GenerateProduct();
            productData.Name = toUpdateProduct.Name;
            productData.Description = toUpdateProduct.Description;
            productData.Price = toUpdateProduct.Price;
            var updatedAt = DateTime.UtcNow;
            productData.UpdatedAt = updatedAt;
            await this._repository.UpdateAsync(productData.Id, productData);

            var updatedProductData = await this._repository.GetByIdAsync(productData.Id);
            Assert.NotNull(updatedProductData);
            Assert.Equal(updatedProductData.Name, toUpdateProduct.Name);
            Assert.Equal(updatedProductData.Description, toUpdateProduct.Description);
            Assert.Equal(updatedProductData.Price.Value, toUpdateProduct.Price.Value);
            var expectedSeconds = new DateTimeOffset(updatedAt).ToUnixTimeSeconds();
            var actualSeconds = new DateTimeOffset(updatedProductData.UpdatedAt.GetValueOrDefault()).ToUnixTimeSeconds();
            Assert.Equal(expectedSeconds, actualSeconds);

        }
    }
}
