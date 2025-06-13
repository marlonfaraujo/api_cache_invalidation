using CacheInvalidation.Api.Entities;
using System.Net.Http.Json;

namespace CacheInvalidation.Integration.Api
{
    public class ProductApiFixture : IDisposable
    {
        public HttpClient Client { get; }
        public Guid ProductId { get; private set; }

        public ProductApiFixture()
        {
            var factory = new CustomWebApplicationFactory();
            Client = factory.CreateClient();
            var productRequest = ProductDtoTestData.GenerateProduct();
            var response = Client.PostAsJsonAsync("/api/products", productRequest).Result;
            var product = response.Content.ReadFromJsonAsync<Product>().Result;
            ProductId = product.Id;
        }

        public void Dispose()
        {
            Client.DeleteAsync($"/api/products/{ProductId}").Wait();
        }
    }
}
