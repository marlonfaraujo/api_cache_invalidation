namespace CacheInvalidation.Api.Application.Abstractions
{
    public interface ICacheConfig
    {
        int ExpirationTimeMinutes { get; set; }
        string ProductCacheKey { get; set; }
        bool ItsToRefresh { get; set; }
    }
}
