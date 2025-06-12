namespace CacheInvalidation.Api
{
    public interface IDatabaseConnection : IDisposable
    {
        Task ExecuteAsync(string sql, params object[]? param);
        Task<T> QueryFirstAsync<T>(string sql, params object[]? param);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[]? param);
    }
}
