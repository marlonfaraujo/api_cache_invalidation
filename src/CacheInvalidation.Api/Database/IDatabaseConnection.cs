namespace CacheInvalidation.Api.Database
{
    public interface IDatabaseConnection
    {
        Task ExecuteAsync(string sql, CancellationToken cancellationToken = default, params object[]? param);
        Task<T> QueryFirstAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null);
    }
}
