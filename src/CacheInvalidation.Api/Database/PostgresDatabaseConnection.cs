using Dapper;
using Npgsql;

namespace CacheInvalidation.Api.Database
{
    public class PostgresDatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public PostgresDatabaseConnection(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task ExecuteAsync(string sql, params object[]? param)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, param);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object? param = null)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, param);
        }
    }
}
