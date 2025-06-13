namespace CacheInvalidation.Api.Database
{
    public interface IDatabaseConnection
    {
        Task ExecuteAsync(string sql, params object[]? param);
        Task<T> QueryFirstAsync<T>(string sql, object? param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
    }
}
