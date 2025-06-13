using StackExchange.Redis;
using System.Text.Json;

namespace CacheInvalidation.Api.Database
{
    public class RedisCacheDatabase : ICacheDatabase
    {
        private readonly IDatabase _database;

        public RedisCacheDatabase(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            return await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<bool> RemoverAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
