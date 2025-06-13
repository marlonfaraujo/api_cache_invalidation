using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Entities;
using StackExchange.Redis;
using System.Collections.Generic;

namespace CacheInvalidation.Integration.Database
{
    public class RedisCacheDatabaseTest
    {
        private readonly RedisCacheDatabase _redis;

        public RedisCacheDatabaseTest()
        {
            this._redis = new RedisCacheDatabase(ConnectionMultiplexer.Connect("localhost:6379,password=redis"));
        }

        [Fact(DisplayName = "Given a list of products stored in the cache when searching in redis then it should return results")]
        public async Task Given_AListOfProductsStoredInTheCache_When_SearchingInRedis_Then_ItShouldReturnResults() 
        {
            int length = 5;
            var items = new List<Product>();
            for (int i = 0; i < length; i++)
            {
                var product = ProductTestData.GenerateProduct();
                items.Add(product);
            }
            await this._redis.SetAsync("products", items);
            var cachedItems = await this._redis.GetAsync<IEnumerable<Product>>("products");
            Assert.NotNull(cachedItems);
            Assert.NotEmpty(cachedItems);
        }
    }
}
