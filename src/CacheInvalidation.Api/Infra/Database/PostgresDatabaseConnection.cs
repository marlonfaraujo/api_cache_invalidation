using Dapper;
using Npgsql;

namespace CacheInvalidation.Api.Infra.Database
{
    public class PostgresDatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public PostgresDatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task ExecuteAsync(string sql, CancellationToken cancellationToken = default, params object[]? param)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(command);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            var queryResult = await connection.QueryFirstOrDefaultAsync<T>(command);
            return queryResult;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken = default, object? param = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            await connection.OpenAsync(cancellationToken);
            var queryResult = await connection.QueryAsync<T>(command);
            return queryResult;
        }
    }
}
