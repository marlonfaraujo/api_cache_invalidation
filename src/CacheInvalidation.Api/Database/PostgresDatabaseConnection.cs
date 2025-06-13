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

        public async Task ExecuteAsync(string sql, CancellationToken cancellationToken = default, params object[]? param)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(command);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryFirstAsync<T>(command);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null)
        {
            using var connection = new NpgsqlConnection(this._connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryAsync<T>(command);
        }
    }
}
