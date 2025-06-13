using CacheInvalidation.Api.Entities;
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

        [Fact(DisplayName = "Given fake product When trying to send in http request Then return http status of created")]
        public async Task Given_FakeProductRequest_When_TryingToSendInHttpRequest_Then_ReturnHttpStatusOfCreated()
        {
            var productRequest = ProductDtoTestData.GenerateProduct();
            var response = await this._productApiFixture.Client.PostAsJsonAsync("/api/products", productRequest);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Given a registered product When updating your data Then it should return an http status of ok")]
        public async Task Given_RegisteredProduct_When_UpdatingYourData_Then_ItShouldReturnAnHttpStatusOfOk()
        {
            var toUpdateProductRequest = ProductDtoTestData.GenerateProduct();
            var putResponse = await _productApiFixture.Client.PutAsJsonAsync($"/api/products/{this._productApiFixture.ProductId}", toUpdateProductRequest);
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            var productPutResult = putResponse.Content.ReadFromJsonAsync<Product>().Result;
            Assert.NotNull(productPutResult);
        }

        [Fact(DisplayName = "Given a registered product When disabling Then it should return with disabled status")]
        public async Task Given_RegisteredProduct_When_Disabling_Then_ItShouldReturnWithDisableddStatus()
        {
            var postResponse = await _productApiFixture.Client.PostAsJsonAsync($"/api/products/{this._productApiFixture.ProductId}/disable", new {});
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            var getResponse = await _productApiFixture.Client.GetAsync($"/api/products/{this._productApiFixture.ProductId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var productResult = getResponse.Content.ReadFromJsonAsync<Product>().Result;
            Assert.Equal(ProductStatusEnum.Disabled.ToString(), productResult.Status);
        }

        [Fact(DisplayName = "Given a deactivated product When activating it Then it should return with the new activated status")]
        public async Task Given_DisabledProduct_When_ActivatingIt_Then_ShouldReturnWithTheNewActivatedStatus()
        {
            var postResponseDisable = await _productApiFixture.Client.PostAsJsonAsync($"/api/products/{this._productApiFixture.ProductId}/disable", new {});
            Assert.Equal(HttpStatusCode.OK, postResponseDisable.StatusCode);
            var postResponseActive = await _productApiFixture.Client.PostAsJsonAsync($"/api/products/{this._productApiFixture.ProductId}/active", new {});
            Assert.Equal(HttpStatusCode.OK, postResponseActive.StatusCode);

            var getResponse = await _productApiFixture.Client.GetAsync($"/api/products/{this._productApiFixture.ProductId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var productResultActive = getResponse.Content.ReadFromJsonAsync<Product>().Result;
            Assert.Equal(ProductStatusEnum.Actived.ToString(), productResultActive.Status);
        }

        [Fact(DisplayName = "Given a registered product When searching for its identifier Then it should return a non-null result")]
        public async Task Given_RegisteredProduct_When_SearchingForItsIdentifier_Then_ShouldReturnANonNullResult()
        {
            var getResponse = await _productApiFixture.Client.GetAsync($"/api/products/${this._productApiFixture.ProductId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var productResult = getResponse.Content.ReadFromJsonAsync<Product>().Result;
            Assert.NotNull(productResult);
        }

        [Fact(DisplayName = "Given cached products When registering a new product Then this new product should be returned in the cached product search result")]
        public async Task Given_CachedProducts_When_RegisteringANewProduct_Then_ThisNewProductShouldBeReturnedInTheCachedProductSearchResult()
        {
            var getResponse = await _productApiFixture.Client.GetAsync($"/api/products-cache");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var productItemsResult = getResponse.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
            Assert.NotNull(productItemsResult);
            Assert.NotEmpty(productItemsResult);

            var newProductRequest = ProductDtoTestData.GenerateProduct();
            var postProductResponse = await this._productApiFixture.Client.PostAsJsonAsync("/api/products", newProductRequest);
            Assert.Equal(HttpStatusCode.Created, postProductResponse.StatusCode);
            var newProductResult = postProductResponse.Content.ReadFromJsonAsync<Product>().Result;

            var getCachedResponse = await _productApiFixture.Client.GetAsync($"/api/products-cache");
            Assert.Equal(HttpStatusCode.OK, getCachedResponse.StatusCode);
            var productItemsCachedResult = getCachedResponse.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
            Assert.NotNull(productItemsCachedResult);
            Assert.NotEmpty(productItemsCachedResult);
            Assert.True(productItemsCachedResult.Any(product => product.Id == newProductResult.Id));
        }
    }
}
