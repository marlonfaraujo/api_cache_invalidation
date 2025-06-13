namespace CacheInvalidation.Api.Database
{
    public interface ICacheDatabase
    {
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetAsync<T>(string key);
        Task<bool> RemoverAsync(string key);
    }
}
