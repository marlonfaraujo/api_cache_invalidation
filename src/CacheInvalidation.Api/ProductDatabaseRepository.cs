
namespace CacheInvalidation.Api
{
    public class ProductDatabaseRepository : IProductRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public ProductDatabaseRepository(IDatabaseConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task CreateAsync(Product product)
        {
            var sql = @"insert into products (id, name, description, status, price, created_at) values
                (@Id, @Name, @Description, @Status, @Price, @CreatedAt)";
            await this._dbConnection.ExecuteAsync(sql, product);
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var sql = "select * from producs where deleted_at is null";
            return await this._dbConnection.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var sql = "select * from producs where id = @Id and deleted_at is null";
            return await this._dbConnection.QueryFirstAsync<Product>(sql, new { Id = id });
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            var sql = @"update products set status = @Status, updated_at = @UpdatedAt, deleted_at = @DeletedAt where id = @Id";
            await this._dbConnection.ExecuteAsync(sql, 
                new { 
                    Status = product.Status, 
                    UpdatedAt = product.UpdatedAt, 
                    DeletedAt = product.DeletedAt, 
                    Id = id 
                });
        }
    }
}
