using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Repositories
{
    public class ProductDatabaseRepository : IProductRepository
    {
        private readonly IDatabaseConnection _postgresDb;

        public ProductDatabaseRepository(IDatabaseConnection postgresDb) 
        {
            this._postgresDb = postgresDb;
        }

        public async Task CreateAsync(Product product)
        {
            var sql = @"insert into products (id, name, description, status, price, created_at) values
            (@Id, @Name, @Description, @Status, @Price, @CreatedAt)";
            await this._postgresDb.ExecuteAsync(sql, product);
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where deleted_at is null";
            return await this._postgresDb.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where id = @Id and deleted_at is null";
            return await this._postgresDb.QueryFirstAsync<Product>(sql, new { Id = id });
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            var sql = @"update products set name = @Name, description = @Description, price = @Price, status = @Status, updated_at = @UpdatedAt, deleted_at = @DeletedAt where id = @Id";
            await this._postgresDb.ExecuteAsync(sql, 
                new {
                    product.Name,
                    product.Description,
                    product.Price.Value,
                    product.Status, 
                    product.UpdatedAt, 
                    product.DeletedAt, 
                    Id = id 
                });
        }
    }
}
