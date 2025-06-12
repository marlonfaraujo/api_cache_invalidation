using Dapper;
using Npgsql;
using System.Data;

namespace CacheInvalidation.Api
{
    public class PostgresDatabaseConnection : IDatabaseConnection
    {
        private readonly IDbConnection _connection;

        public PostgresDatabaseConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            this._connection = new NpgsqlConnection(connectionString);
        }

        public async Task ExecuteAsync(string sql, params object[]? param)
        {
            await this._connection.ExecuteAsync(sql, param);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, params object[]? param)
        {
            return await this._connection.QueryFirstAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[]? param)
        {
            return await this._connection.QueryAsync<T>(sql, param);
        }

        public void Dispose()
        {
            this._connection.Dispose();
            this._connection.Close();
        }
    }
}
