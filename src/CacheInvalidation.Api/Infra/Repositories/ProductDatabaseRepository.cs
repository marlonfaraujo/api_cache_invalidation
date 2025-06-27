using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Domain.Repositories;
using CacheInvalidation.Api.Infra.Database;
using CacheInvalidation.Api.Infra.Dtos;

namespace CacheInvalidation.Api.Infra.Repositories
{
    public class ProductDatabaseRepository : IProductRepository
    {
        private readonly IDatabaseConnection _postgresDb;

        public ProductDatabaseRepository(IDatabaseConnection postgresDb) 
        {
            _postgresDb = postgresDb;
        }

        public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var sql = @"insert into products (id, name, description, status, price, created_at) values
            (@Id, @Name, @Description, @Status, @Price, @CreatedAt)";
            await _postgresDb.ExecuteAsync(sql, 
                cancellationToken, 
                new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Status,
                    Price = product.Price.Value,
                    product.CreatedAt
                });
        }

        public async Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken = default)
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where deleted_at is null";
            var productQueryResultItems = await _postgresDb.QueryAsync<ProductQueryResult>(sql, cancellationToken);
            return productQueryResultItems.Select(product => 
                Product.Create(product.Id, product.Name, product.Description, product.Status, product.Price, product.CreatedAt, product.UpdatedAt));
        }

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sql = "select id, name, description, status, price, created_at createdAt, updated_at updatedAt from products where id = @Id";
            var productQueryResult = await _postgresDb.QueryFirstAsync<ProductQueryResult>(sql, cancellationToken, new { Id = id });
            if (productQueryResult == null)
            {
                return null!;
            }
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
            await _postgresDb.ExecuteAsync(sql, 
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
