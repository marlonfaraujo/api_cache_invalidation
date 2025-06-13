using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
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

        public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var sql = @"insert into products (id, name, description, status, price, created_at) values
            (@Id, @Name, @Description, @Status, @Price, @CreatedAt)";
            await this._postgresDb.ExecuteAsync(sql, 
                cancellationToken, 
                new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Status = product.Status,
                    Price = product.Price.Value,
                    CreatedAt = product.CreatedAt
                });
        }

        public async Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken = default)
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where deleted_at is null";
            var productQueryResultItems = await this._postgresDb.QueryAsync<ProductQueryResult>(sql, cancellationToken);
            return productQueryResultItems.Select(product => 
                Product.Create(product.Id, product.Name, product.Description, product.Status, product.Price, product.CreatedAt, product.UpdatedAt));
        }

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where id = @Id";
            var productQueryResult = await this._postgresDb.QueryFirstAsync<ProductQueryResult>(sql, cancellationToken, new { Id = id });
            return Product.Create(productQueryResult.Id, 
                productQueryResult.Name, 
                productQueryResult.Description, 
                productQueryResult.Status, 
                productQueryResult.Price, 
                productQueryResult.CreatedAt, 
                productQueryResult.UpdatedAt);
        }

        public async Task UpdateAsync(Guid id, Product product, CancellationToken cancellationToken = default)
        {
            var sql = @"update products set name = @Name, description = @Description, price = @Price, status = @Status, updated_at = @UpdatedAt, deleted_at = @DeletedAt where id = @Id";
            await this._postgresDb.ExecuteAsync(sql, 
                cancellationToken,
                new {
                    product.Name,
                    product.Description,
                    Price = product.Price.Value,
                    product.Status, 
                    product.UpdatedAt, 
                    product.DeletedAt, 
                    Id = id 
                });
        }
    }
}
