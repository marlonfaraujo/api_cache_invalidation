using CacheInvalidation.Api.Application.Abstractions;

namespace CacheInvalidation.Api.Dtos
{
    public class CacheConfig : ICacheConfig
    {
        public int ExpirationTimeMinutes { get; set; }
        public string ProductCacheKey { get; set; }
        public bool ItsToRefresh { get; set; }
        public CacheConfig()
        {
        }
        public CacheConfig(int expirationTimeMinutes, string productCacheKey, bool? itsToRefresh = false)
        {
            ExpirationTimeMinutes = expirationTimeMinutes;
            ProductCacheKey = productCacheKey;
            ItsToRefresh = itsToRefresh.GetValueOrDefault();
        }
    }
}
