using System.Net;
using System.Net.Http.Json;

namespace CacheInvalidation.Integration.Api
{
    public class ProductApiTest : IClassFixture<ProductApiFixture>
    {

        private readonly ProductApiFixture _productApiFixture;

        public ProductApiTest(ProductApiFixture productApiFixture)
        {
            this._productApiFixture = productApiFixture;
        }

        [Fact(DisplayName = "POST /api/products should return Created when product is valid")]
        public async Task CreateProduct_ReturnsCreated()
        {
            var productRequest = ProductTestData.GenerateProduct();

            var response = await _productApiFixture.Client.PostAsJsonAsync("/api/products", productRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
