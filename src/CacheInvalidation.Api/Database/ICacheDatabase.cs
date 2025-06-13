namespace CacheInvalidation.Api.Database
{
    public interface ICacheDatabase
    {
        Task<bool> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default, TimeSpan? expiry = null);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task<bool> RemoverAsync(string key, CancellationToken cancellationToken = default);
    }
}
