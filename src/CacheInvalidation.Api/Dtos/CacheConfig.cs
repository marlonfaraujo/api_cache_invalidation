namespace CacheInvalidation.Api.Dtos
{
    public record CacheConfig(int ExpirationTimeMinutes, string ProductCacheKey, bool? ItsToRefresh = false)
    {
    }
}
